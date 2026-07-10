using AgenceBlazor.Data;
using AgenceBlazor.Models;
using Microsoft.EntityFrameworkCore;

namespace AgenceBlazor.Services
{
    public class TreasuryService
    {
        private readonly ApplicationDbContext _context;

        public TreasuryService(ApplicationDbContext context) => _context = context;

        // ==================== MAIN TREASURY ====================
        public async Task<List<TreasuryTransaction>> GetAllAsync()
        {
            return await _context.TreasuryTransactions
                .OrderByDescending(t => t.TransactionDate)
                .ThenByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<decimal> GetBalanceAsync()
        {
            var deposits = await _context.TreasuryTransactions
                .Where(t => t.Type == "deposit" || t.Type == "transfer_from_owner")
                .SumAsync(t => t.Amount);
            var withdrawals = await _context.TreasuryTransactions
                .Where(t => t.Type == "withdrawal")
                .SumAsync(t => t.Amount);
            var expenses = await _context.TreasuryTransactions
                .Where(t => t.Type == "expense")
                .SumAsync(t => t.Amount);
            var transfersToOwner = await _context.TreasuryTransactions
                .Where(t => t.Type == "transfer_to_owner")
                .SumAsync(t => t.Amount);
            return deposits - withdrawals - expenses - transfersToOwner;
        }

        public async Task<decimal> GetDepositsTotalAsync()
        {
            return await _context.TreasuryTransactions
                .Where(t => t.Type == "deposit" || t.Type == "transfer_from_owner")
                .SumAsync(t => t.Amount);
        }

        public async Task<decimal> GetWithdrawalsTotalAsync()
        {
            return await _context.TreasuryTransactions
                .Where(t => t.Type == "withdrawal")
                .SumAsync(t => t.Amount);
        }

        public async Task<decimal> GetExpensesTotalAsync()
        {
            return await _context.TreasuryTransactions
                .Where(t => t.Type == "expense")
                .SumAsync(t => t.Amount);
        }

        public async Task<bool> CreateAsync(TreasuryTransaction treasury)
        {
            try
            {
                _context.TreasuryTransactions.Add(treasury);
                await _context.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var item = await _context.TreasuryTransactions.FindAsync(id);
                if (item == null) return false;
                _context.TreasuryTransactions.Remove(item);
                await _context.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        // ==================== OWNER TREASURY ====================
        public async Task<List<OwnerTreasuryTransaction>> GetOwnerTransactionsAsync()
        {
            return await _context.OwnerTreasuryTransactions
                .OrderByDescending(t => t.TransactionDate)
                .ThenByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<(decimal bankBalance, decimal cashBalance, decimal totalBalance)> GetOwnerBalanceAsync()
        {
            var bankDeposits = await _context.OwnerTreasuryTransactions
                .Where(t => t.Source == "bank" && (t.Type == "deposit" || t.Type == "transfer_from_main"))
                .SumAsync(t => t.Amount);

            var bankWithdrawals = await _context.OwnerTreasuryTransactions
                .Where(t => t.Source == "bank" && (t.Type == "withdrawal" || t.Type == "transfer_to_main"))
                .SumAsync(t => t.Amount);

            var cashDeposits = await _context.OwnerTreasuryTransactions
                .Where(t => t.Source == "cash" && (t.Type == "deposit" || t.Type == "transfer_from_main"))
                .SumAsync(t => t.Amount);

            var cashWithdrawals = await _context.OwnerTreasuryTransactions
                .Where(t => t.Source == "cash" && (t.Type == "withdrawal" || t.Type == "transfer_to_main"))
                .SumAsync(t => t.Amount);

            var bankBalance = bankDeposits - bankWithdrawals;
            var cashBalance = cashDeposits - cashWithdrawals;

            return (bankBalance, cashBalance, bankBalance + cashBalance);
        }

        public async Task<bool> CreateOwnerTransactionAsync(OwnerTreasuryTransaction transaction)
        {
            try
            {
                _context.OwnerTreasuryTransactions.Add(transaction);
                await _context.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public async Task<bool> DeleteOwnerTransactionAsync(int id)
        {
            try
            {
                var item = await _context.OwnerTreasuryTransactions.FindAsync(id);
                if (item == null) return false;
                _context.OwnerTreasuryTransactions.Remove(item);
                await _context.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        // ==================== TRANSFERS BETWEEN TREASURIES ====================
        public async Task<bool> TransferFromOwnerBankToMain(decimal amount, string description, DateTime date, string notes = "")
        {
            return await TransferToMain("bank", amount, description, date, notes);
        }

        public async Task<bool> TransferFromOwnerCashToMain(decimal amount, string description, DateTime date, string notes = "")
        {
            return await TransferToMain("cash", amount, description, date, notes);
        }

        private async Task<bool> TransferToMain(string source, decimal amount, string description, DateTime date, string notes = "")
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Add to main treasury
                var mainEntry = new TreasuryTransaction
                {
                    Type = "transfer_from_owner",
                    Amount = amount,
                    Description = $"{description} (من {GetSourceLabel(source)} المالك)",
                    TransactionDate = date,
                    Notes = notes
                };
                _context.TreasuryTransactions.Add(mainEntry);
                await _context.SaveChangesAsync();

                // Deduct from owner treasury
                var ownerEntry = new OwnerTreasuryTransaction
                {
                    Type = "transfer_to_main",
                    Source = source,
                    Amount = amount,
                    Description = description,
                    TransactionDate = date,
                    Notes = notes,
                    MainTreasuryRefId = mainEntry.Id
                };
                _context.OwnerTreasuryTransactions.Add(ownerEntry);
                await _context.SaveChangesAsync();

                mainEntry.OwnerTreasuryRefId = ownerEntry.Id;
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> TransferFromMainToOwnerBank(decimal amount, string description, DateTime date, string notes = "")
        {
            return await TransferFromMain("bank", amount, description, date, notes);
        }

        public async Task<bool> TransferFromMainToOwnerCash(decimal amount, string description, DateTime date, string notes = "")
        {
            return await TransferFromMain("cash", amount, description, date, notes);
        }

        private async Task<bool> TransferFromMain(string source, decimal amount, string description, DateTime date, string notes = "")
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Deduct from main treasury
                var mainEntry = new TreasuryTransaction
                {
                    Type = "transfer_to_owner",
                    Amount = amount,
                    Description = $"{description} (إلى {GetSourceLabel(source)} المالك)",
                    TransactionDate = date,
                    Notes = notes
                };
                _context.TreasuryTransactions.Add(mainEntry);
                await _context.SaveChangesAsync();

                // Add to owner treasury
                var ownerEntry = new OwnerTreasuryTransaction
                {
                    Type = "transfer_from_main",
                    Source = source,
                    Amount = amount,
                    Description = description,
                    TransactionDate = date,
                    Notes = notes,
                    MainTreasuryRefId = mainEntry.Id
                };
                _context.OwnerTreasuryTransactions.Add(ownerEntry);
                await _context.SaveChangesAsync();

                mainEntry.OwnerTreasuryRefId = ownerEntry.Id;
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        private string GetSourceLabel(string source)
        {
            return source == "bank" ? "البنك" : "النقد";
        }

        public async Task<(decimal bankBalance, decimal cashBalance, decimal totalBalance, decimal expensesTotal)> GetOwnerBalanceWithExpensesAsync()
        {
            var bankDeposits = await _context.OwnerTreasuryTransactions
                .Where(t => t.Source == "bank" && (t.Type == "deposit" || t.Type == "transfer_from_main"))
                .SumAsync(t => t.Amount);

            var bankWithdrawals = await _context.OwnerTreasuryTransactions
                .Where(t => t.Source == "bank" && (t.Type == "withdrawal" || t.Type == "expense" || t.Type == "transfer_to_main"))
                .SumAsync(t => t.Amount);

            var cashDeposits = await _context.OwnerTreasuryTransactions
                .Where(t => t.Source == "cash" && (t.Type == "deposit" || t.Type == "transfer_from_main"))
                .SumAsync(t => t.Amount);

            var cashWithdrawals = await _context.OwnerTreasuryTransactions
                .Where(t => t.Source == "cash" && (t.Type == "withdrawal" || t.Type == "expense" || t.Type == "transfer_to_main"))
                .SumAsync(t => t.Amount);

            var expensesTotal = await _context.OwnerTreasuryTransactions
                .Where(t => t.Type == "expense")
                .SumAsync(t => t.Amount);

            var bankBalance = bankDeposits - bankWithdrawals;
            var cashBalance = cashDeposits - cashWithdrawals;

            return (bankBalance, cashBalance, bankBalance + cashBalance, expensesTotal);
        }

        // Add agency payment to main treasury
        public async Task<bool> AddAgencyPaymentToTreasuryAsync(Guid agencyId, string agencyName, decimal amount, string paymentMethod, string notes = "")
        {
            try
            {
                var transaction = new TreasuryTransaction
                {
                    Type = "deposit",
                    Amount = amount,
                    Description = $"دفعة من وكالة {agencyName}",
                    ReferenceType = "payment",
                    TransactionDate = DateTime.UtcNow,
                    Notes = notes,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.TreasuryTransactions.Add(transaction);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
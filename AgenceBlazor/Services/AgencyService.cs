using AgenceBlazor.Data;
using AgenceBlazor.Models;
using Microsoft.EntityFrameworkCore;

namespace AgenceBlazor.Services
{
    public class AgencyService
    {
        private readonly ApplicationDbContext _context;

        public AgencyService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all active agencies
        public async Task<List<Agency>> GetAllAgenciesAsync()
        {
            return await _context.Agencies
                .Where(a => a.Status == "Active")
                .OrderBy(a => a.AgencyName)
                .ToListAsync();
        }

        // Get all agencies (including inactive)
        public async Task<List<Agency>> GetAllAgenciesAdminAsync()
        {
            return await _context.Agencies
                .OrderBy(a => a.AgencyName)
                .ToListAsync();
        }

        // Get agency by ID
        public async Task<Agency> GetAgencyByIdAsync(Guid agencyId)
        {
            return await _context.Agencies.FindAsync(agencyId);
        }

        // Search agencies
        public async Task<List<Agency>> SearchAgenciesAsync(string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
                return await GetAllAgenciesAsync();

            searchQuery = searchQuery.ToLower();

            return await _context.Agencies
                .Where(a =>
                    a.AgencyName.ToLower().Contains(searchQuery) ||
                    (a.City != null && a.City.ToLower().Contains(searchQuery)) ||
                    (a.AgencyType != null && a.AgencyType.ToLower().Contains(searchQuery)) ||
                    (a.Address != null && a.Address.ToLower().Contains(searchQuery)) ||
                    (a.Phone != null && a.Phone.Contains(searchQuery)))
                .OrderBy(a => a.AgencyName)
                .ToListAsync();
        }

        // Create agency
        public async Task<bool> CreateAgencyAsync(Agency agency)
        {
            try
            {
                if (string.IsNullOrEmpty(agency.Status))
                    agency.Status = "Active";

                _context.Agencies.Add(agency);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating agency: {ex.Message}");
                return false;
            }
        }

        // Update agency
        public async Task<bool> UpdateAgencyAsync(Agency agency)
        {
            try
            {
                var existing = await _context.Agencies.FindAsync(agency.AgencyId);
                if (existing == null)
                    return false;

                existing.AgencyName = agency.AgencyName;
                existing.AgencyType = agency.AgencyType;
                existing.Status = agency.Status;
                existing.CommissionRate = agency.CommissionRate;
                existing.ContractDate = agency.ContractDate;
                existing.PilgrimsCount = agency.PilgrimsCount;
                existing.DebtAmount = agency.DebtAmount;
                existing.PaidAmount = agency.PaidAmount;
                existing.Phone = agency.Phone;
                existing.Email = agency.Email;
                existing.Address = agency.Address;
                existing.City = agency.City;
                existing.Notes = agency.Notes;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating agency: {ex.Message}");
                return false;
            }
        }
        // Update agency financials only
        public async Task<bool> UpdateAgencyFinancialsAsync(Guid agencyId, decimal debtAmount, int pilgrimsCount)
        {
            try
            {
                var existing = await _context.Agencies.FindAsync(agencyId);
                if (existing == null) return false;

                existing.DebtAmount += debtAmount;
                existing.PilgrimsCount += pilgrimsCount;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating agency financials: {ex.Message}");
                return false;
            }
        }
        // Delete agency (soft delete)
        public async Task<bool> DeleteAgencyAsync(Guid agencyId)
        {
            try
            {
                var agency = await _context.Agencies.FindAsync(agencyId);
                if (agency == null)
                    return false;

                agency.Status = "Inactive";
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting agency: {ex.Message}");
                return false;
            }
        }

        // Get agency stats
        public async Task<int> GetActiveAgenciesCountAsync()
        {
            return await _context.Agencies.CountAsync(a => a.Status == "Active");
        }

        public async Task<decimal> GetTotalDebtAsync()
        {
            return await _context.Agencies
                .Where(a => a.Status == "Active")
                .SumAsync(a => a.RemainingAmount);
        }

        public async Task<decimal> GetTotalPaidAsync()
        {
            return await _context.Agencies
                .Where(a => a.Status == "Active")
                .SumAsync(a => a.PaidAmount);
        }

        // Update payment
        public async Task<bool> AddPaymentAsync(Guid agencyId, decimal amount)
        {
            try
            {
                var agency = await _context.Agencies.FindAsync(agencyId);
                if (agency == null)
                    return false;

                agency.PaidAmount += amount;
                // RemainingAmount is computed (DebtAmount - PaidAmount), so it auto-updates

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding payment: {ex.Message}");
                return false;
            }
        }
        // Add this method to AgencyService
        public async Task<Dictionary<Guid, decimal>> GetReductionsByAgencyAsync()
        {
            return await _context.AgencyBookings
                .Where(b => b.AgencyId != null)
                .GroupBy(b => b.AgencyId.Value)
                .Select(g => new { AgencyId = g.Key, TotalReduction = g.Sum(b => b.Reduction) })
                .ToDictionaryAsync(x => x.AgencyId, x => x.TotalReduction);
        }
    }
}
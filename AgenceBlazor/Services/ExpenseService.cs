using AgenceBlazor.Data;
using AgenceBlazor.Models;
using Microsoft.EntityFrameworkCore;

namespace AgenceBlazor.Services
{
    public class ExpenseService
    {
        private readonly ApplicationDbContext _context;

        public ExpenseService(ApplicationDbContext context) => _context = context;

        public async Task<List<Expense>> GetAllAsync()
        {
            return await _context.Expenses
                .OrderByDescending(e => e.ExpenseDate)
                .ToListAsync();
        }

        public async Task<List<Expense>> GetByTripAsync(int tripId)
        {
            return await _context.Expenses
                .Where(e => e.TripId == tripId)
                .OrderByDescending(e => e.ExpenseDate)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalAsync()
        {
            return await _context.Expenses.SumAsync(e => e.Amount);
        }

        public async Task<bool> CreateAsync(Expense expense)
        {
            try
            {
                var algeriaTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "W. Central Africa Standard Time");
                expense.CreatedAt = DateTime.SpecifyKind(algeriaTime, DateTimeKind.Utc);
                expense.UpdatedAt = DateTime.SpecifyKind(algeriaTime, DateTimeKind.Utc);
                expense.ExpenseDate = DateTime.SpecifyKind(expense.ExpenseDate, DateTimeKind.Utc);
                _context.Expenses.Add(expense);
                await _context.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public async Task<bool> UpdateAsync(Expense expense)
        {
            try
            {
                var existing = await _context.Expenses.FindAsync(expense.Id);
                if (existing == null) return false;
                existing.Amount = expense.Amount;
                existing.Description = expense.Description;
                existing.Category = expense.Category;

                // Convert to Algeria time (GMT+1) then specify as UTC for PostgreSQL
                var algeriaTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(expense.ExpenseDate, "W. Central Africa Standard Time");
                existing.ExpenseDate = DateTime.SpecifyKind(algeriaTime, DateTimeKind.Utc);

                existing.TripId = expense.TripId;
                existing.Notes = expense.Notes;
                await _context.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var expense = await _context.Expenses.FindAsync(id);
                if (expense == null) return false;
                _context.Expenses.Remove(expense);
                await _context.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        //public async Task<List<string>> GetCategoriesAsync()
        //{
        //    return await _context.ExpenseCategories
        //        .Select(c => c.Name)
        //        .OrderBy(c => c)
        //        .ToListAsync();
        //}
        public async Task<List<string>> GetCategoriesAsync()
        {
            return await _context.Expenses
                .Select(e => e.Category)
                .Distinct()
                .Where(c => c != null && c != "")
                .OrderBy(c => c)
                .ToListAsync();
        }
    }
}
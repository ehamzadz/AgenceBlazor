using AgenceBlazor.Data;
using AgenceBlazor.Models;
using Microsoft.EntityFrameworkCore;

namespace AgenceBlazor.Services
{
    public class DirectPilgrimService
    {
        private readonly ApplicationDbContext _context;

        public DirectPilgrimService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DirectPilgrim> CreateAsync(DirectPilgrim pilgrim)
        {
            pilgrim.CreatedAt = DateTime.UtcNow;
            pilgrim.UpdatedAt = DateTime.UtcNow;

            _context.DirectPilgrims.Add(pilgrim);
            await _context.SaveChangesAsync();
            return pilgrim;
        }

        public async Task<List<DirectPilgrim>> GetByTripIdAsync(int tripId)
        {
            return await _context.DirectPilgrims
                .Include(p => p.FamilyMembers)
                .Where(p => p.TripId == tripId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<DirectPilgrim> GetByIdAsync(int id)
        {
            return await _context.DirectPilgrims
                .Include(p => p.FamilyMembers)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<int> GetTotalPilgrimsByTripAsync(int tripId)
        {
            return await _context.DirectPilgrims
                .Where(p => p.TripId == tripId && p.Status == "confirmed")
                .SumAsync(p => p.TotalPilgrims);
        }

        public async Task<decimal> GetTotalRevenueByTripAsync(int tripId)
        {
            return await _context.DirectPilgrims
                .Where(p => p.TripId == tripId && p.Status == "confirmed")
                .SumAsync(p => p.NetAmount);
        }

        public async Task<bool> AddPaymentAsync(int pilgrimId, decimal amount)
        {
            try
            {
                var pilgrim = await _context.DirectPilgrims.FindAsync(pilgrimId);
                if (pilgrim == null) return false;

                pilgrim.PaidAmount += amount;
                pilgrim.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        // Add this method
        // Services/DirectPilgrimService.cs - Simplified version
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var pilgrim = await _context.DirectPilgrims.FindAsync(id);

                if (pilgrim == null) return false;

                _context.DirectPilgrims.Remove(pilgrim); // Cascade will handle family members
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting pilgrim: {ex.Message}");
                return false;
            }
        }
    }
}
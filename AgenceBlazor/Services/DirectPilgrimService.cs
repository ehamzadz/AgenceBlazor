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
    }
}
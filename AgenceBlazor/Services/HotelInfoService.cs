using AgenceBlazor.Data;
using AgenceBlazor.Models;
using Microsoft.EntityFrameworkCore;

namespace AgenceBlazor.Services
{
    public class HotelInfoService
    {
        private readonly ApplicationDbContext _context;
        public HotelInfoService(ApplicationDbContext context) => _context = context;

        public async Task<List<HotelInfo>> GetAllAsync() =>
            await _context.HotelInfos.OrderBy(h => h.Name).ToListAsync();

        public async Task<bool> CreateAsync(HotelInfo hotel)
        {
            try { _context.HotelInfos.Add(hotel); await _context.SaveChangesAsync(); return true; }
            catch { return false; }
        }

        public async Task<bool> UpdateAsync(HotelInfo hotel)
        {
            try
            {
                var existing = await _context.HotelInfos.FindAsync(hotel.Id);
                if (existing == null) return false;
                existing.Name = hotel.Name;
                existing.Location = hotel.Location;
                existing.DistanceFromHaram = hotel.DistanceFromHaram;
                existing.ClientName = hotel.ClientName;
                await _context.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var hotel = await _context.HotelInfos.FindAsync(id);
                if (hotel == null) return false;
                hotel.IsActive = false;
                await _context.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }
    }
}
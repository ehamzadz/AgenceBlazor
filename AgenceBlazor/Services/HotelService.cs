using AgenceBlazor.Data;
using AgenceBlazor.Models;
using Microsoft.EntityFrameworkCore;

namespace AgenceBlazor.Services
{
    public class HotelService
    {
        private readonly ApplicationDbContext _context;

        public HotelService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Hotel>> GetHotelsByTripIdAsync(int tripId)
        {
            return await _context.Hotels
                .Where(h => h.TripId == tripId)
                .OrderBy(h => h.Name)
                .ToListAsync();
        }

        public async Task<List<string>> GetHotelNamesByTripIdAsync(int tripId)
        {
            return await _context.Hotels
                .Where(h => h.TripId == tripId)
                .Select(h => h.Name)
                .Distinct()
                .OrderBy(n => n)
                .ToListAsync();
        }

        public async Task<Hotel> GetHotelByNameAndTripAsync(string name, int tripId)
        {
            return await _context.Hotels
                .FirstOrDefaultAsync(h => h.Name == name && h.TripId == tripId);
        }

        //public async Task<bool> AddHotelAsync(Hotel hotel)
        //{
        //    try
        //    {
        //        _context.Hotels.Add(hotel);
        //        await _context.SaveChangesAsync();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error adding hotel: {ex.Message}");
        //        return false;
        //    }
        //}
        public async Task<bool> AddHotelAsync(Hotel hotel)
        {
            try
            {
                hotel.CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
                hotel.UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
                _context.Hotels.Add(hotel);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding hotel: {ex.Message}");
                return false;
            }
        }

        public async Task<int> GetDistinctHotelCountAsync()
        {
            return await _context.Hotels.Select(h => h.Name).Distinct().CountAsync();
        }

        public async Task<List<Hotel>> GetAllHotelsWithTripsAsync()
        {
            return await _context.Hotels
                .Include(h => h.Trip)
                .Where(h => h.IsActive)
                .OrderBy(h => h.Trip.DepartureDate)
                .ThenBy(h => h.Name)
                .ToListAsync();
        }

        public async Task<bool> UpdateHotelAsync(Hotel hotel)
        {
            try
            {
                var existing = await _context.Hotels.FindAsync(hotel.Id);
                if (existing == null) return false;
                existing.Name = hotel.Name;
                existing.GroupPrice = hotel.GroupPrice;
                existing.QuadruplePrice = hotel.QuadruplePrice;
                existing.TriplePrice = hotel.TriplePrice;
                existing.DoublePrice = hotel.DoublePrice;
                existing.ChildPrice = hotel.ChildPrice;
                existing.InfantPrice = hotel.InfantPrice;
                await _context.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public async Task<bool> DeleteHotelAsync(int id)
        {
            try
            {
                var hotel = await _context.Hotels.FindAsync(id);
                if (hotel == null) return false;
                hotel.IsActive = false;
                await _context.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }
    }
}
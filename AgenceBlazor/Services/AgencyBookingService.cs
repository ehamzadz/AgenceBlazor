using AgenceBlazor.Data;
using AgenceBlazor.Models;
using Microsoft.EntityFrameworkCore;

namespace AgenceBlazor.Services
{
    public class AgencyBookingService
    {
        private readonly ApplicationDbContext _context;

        public AgencyBookingService(ApplicationDbContext context)
        {
            _context = context;
        }

        //public async Task<bool> CreateAsync(AgencyBooking booking)
        //{
        //    try
        //    {
        //        _context.AgencyBookings.Add(booking);
        //        await _context.SaveChangesAsync();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error saving agency booking: {ex.Message}");
        //        return false;
        //    }
        //}
        //public async Task<bool> CreateAsync(AgencyBooking booking)
        //{
        //    try
        //    {
        //        // Algeria timezone (GMT+1)
        //        var algeriaTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "W. Central Africa Standard Time");
        //        booking.CreatedAt = algeriaTime;
        //        booking.UpdatedAt = algeriaTime;
        //        _context.AgencyBookings.Add(booking);
        //        await _context.SaveChangesAsync();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error saving agency booking: {ex.Message}");
        //        return false;
        //    }
        //}
        public async Task<bool> CreateAsync(AgencyBooking booking)
        {
            try
            {
                //        // Algeria timezone (GMT+1)
                        var algeriaTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "W. Central Africa Standard Time");
                booking.CreatedAt = algeriaTime;
                booking.UpdatedAt = algeriaTime;
                //booking.CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
                //booking.UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);

                _context.AgencyBookings.Add(booking);
                await _context.SaveChangesAsync();

                // Force update reduction with raw SQL
                if (booking.Reduction != 0)
                {
                    await _context.Database.ExecuteSqlRawAsync(
                        "UPDATE agency_bookings SET reduction = {0} WHERE id = {1}",
                        booking.Reduction,
                        booking.Id);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving agency booking: {ex.Message}");
                return false;
            }
        }
        public async Task<List<AgencyBooking>> GetAllAsync()
        {
            return await _context.AgencyBookings
                .Include(b => b.Trip)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
        }
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var booking = await _context.AgencyBookings.FindAsync(id);
                if (booking == null) return false;
                _context.AgencyBookings.Remove(booking);
                await _context.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public async Task<bool> UpdateAsync(AgencyBooking booking)
        {
            try
            {
                booking.UpdatedAt = DateTime.UtcNow;
                _context.AgencyBookings.Update(booking);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating booking: {ex.Message}");
                return false;
            }
        }
    }
}
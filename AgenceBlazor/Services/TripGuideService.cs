// Services/TripGuideService.cs
using AgenceBlazor.Data;
using AgenceBlazor.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AgenceBlazor.Services
{
    public interface ITripGuideService
    {
        Task<TripGuide?> GetGuideByTripIdAsync(int tripId);
        Task<TripGuide> CreateOrUpdateGuideAsync(TripGuide guide);
        Task<bool> DeleteGuideAsync(int tripId);
    }

    public class TripGuideService : ITripGuideService
    {
        private readonly ApplicationDbContext _context;

        public TripGuideService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TripGuide?> GetGuideByTripIdAsync(int tripId)
        {
            try
            {
                return await _context.TripGuides
                    .FirstOrDefaultAsync(g => g.TripId == tripId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetGuideByTripIdAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<TripGuide> CreateOrUpdateGuideAsync(TripGuide guide)
        {
            try
            {
                var existing = await GetGuideByTripIdAsync(guide.TripId);

                if (existing != null)
                {
                    // Update existing
                    existing.Name = guide.Name;
                    existing.Agency = guide.Agency;
                    existing.GrantAmount = guide.GrantAmount;
                    existing.UpdatedAt = DateTime.UtcNow;
                    _context.TripGuides.Update(existing);
                    await _context.SaveChangesAsync();
                    return existing;
                }
                else
                {
                    // Create new
                    guide.CreatedAt = DateTime.UtcNow;
                    guide.UpdatedAt = DateTime.UtcNow;
                    _context.TripGuides.Add(guide);
                    await _context.SaveChangesAsync();
                    return guide;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreateOrUpdateGuideAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteGuideAsync(int tripId)
        {
            try
            {
                var guide = await GetGuideByTripIdAsync(tripId);
                if (guide != null)
                {
                    _context.TripGuides.Remove(guide);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteGuideAsync: {ex.Message}");
                throw;
            }
        }
    }
}
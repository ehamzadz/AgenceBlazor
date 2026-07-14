// Services/TripAirlinePricingService.cs
using AgenceBlazor.Data;
using AgenceBlazor.Models;
using Microsoft.EntityFrameworkCore;

namespace AgenceBlazor.Services
{
    public class TripAirlinePricingService
    {
        private readonly ApplicationDbContext _context;

        public TripAirlinePricingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TripAirlinePricing?> GetByTripIdAsync(int tripId)
        {
            return await _context.TripAirlinePricings
                .Include(p => p.Trip)
                .FirstOrDefaultAsync(p => p.TripId == tripId);
        }

        public async Task<List<TripAirlinePricing>> GetAllAsync()
        {
            return await _context.TripAirlinePricings
                .Include(p => p.Trip)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<TripAirlinePricing> CreateOrUpdateAsync(TripAirlinePricing pricing)
        {
            var existing = await _context.TripAirlinePricings
                .FirstOrDefaultAsync(p => p.TripId == pricing.TripId);

            if (existing != null)
            {
                existing.AdultPrice = pricing.AdultPrice;
                existing.ChildPrice = pricing.ChildPrice;
                existing.InfantPrice = pricing.InfantPrice;
                existing.FreeSeatsCount = pricing.FreeSeatsCount;
                existing.FreeSeatPrice = pricing.FreeSeatPrice;
                existing.AdultCount = pricing.AdultCount;
                existing.ChildCount = pricing.ChildCount;
                existing.InfantCount = pricing.InfantCount;
                existing.Notes = pricing.Notes;
                existing.IsPaid = pricing.IsPaid;
                existing.PaidDate = pricing.PaidDate;
                existing.PaidAmount = pricing.PaidAmount;
                existing.UpdatedAt = DateTime.UtcNow;

                _context.TripAirlinePricings.Update(existing);
            }
            else
            {
                pricing.CreatedAt = DateTime.UtcNow;
                pricing.UpdatedAt = DateTime.UtcNow;
                _context.TripAirlinePricings.Add(pricing);
            }

            await _context.SaveChangesAsync();
            return pricing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var pricing = await _context.TripAirlinePricings.FindAsync(id);
            if (pricing == null) return false;

            _context.TripAirlinePricings.Remove(pricing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<decimal> GetTotalAirlineCostAsync(int tripId)
        {
            var pricing = await _context.TripAirlinePricings
                .FirstOrDefaultAsync(p => p.TripId == tripId);

            return pricing?.TotalAirlineCost ?? 0;
        }

        public async Task UpdatePaymentAsync(TripAirlinePricing pricing)
        {
            var existing = await _context.TripAirlinePricings.FindAsync(pricing.Id);
            if (existing != null)
            {
                existing.PaidAmount = pricing.PaidAmount;
                existing.IsPaid = pricing.IsPaid;
                existing.PaidDate = pricing.PaidDate;
                existing.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}
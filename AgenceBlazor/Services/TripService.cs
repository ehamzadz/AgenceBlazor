using AgenceBlazor.Data;
using Microsoft.EntityFrameworkCore;
using AgenceBlazor.Models;

namespace AgenceBlazor.Services
{
    public class TripService
    {
        private readonly ApplicationDbContext _context;

        public TripService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Trip>> GetAllTripsAsync()
        {
            return await _context.Trips
                .OrderBy(t => t.DepartureDate)
                .ToListAsync();
        }

        public async Task<List<Trip>> SearchTripsAsync(string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
                return await GetAllTripsAsync();

            searchQuery = searchQuery.ToLower();

            return await _context.Trips
                .Where(t =>
                    t.TripNumber.ToLower().Contains(searchQuery) ||
                    t.TripName.ToLower().Contains(searchQuery) ||
                    t.DepartureFrom.ToLower().Contains(searchQuery) ||
                    t.ArrivalTo.ToLower().Contains(searchQuery) ||
                    t.Airline.ToLower().Contains(searchQuery) ||
                    t.Program.ToLower().Contains(searchQuery) ||
                    t.TripType.ToLower().Equals(searchQuery))
                .OrderBy(t => t.DepartureDate)
                .ToListAsync();
        }

        public async Task<Trip> GetTripByIdAsync(int id)
        {
            return await _context.Trips.FindAsync(id);
        }

        public async Task<int> GetTotalTripsCountAsync()
        {
            return await _context.Trips.CountAsync();
        }

        public async Task<int> GetDirectTripsCountAsync()
        {
            return await _context.Trips.CountAsync(t => t.TripType == "مباشرة");
        }

        public async Task<int> GetIndirectTripsCountAsync()
        {
            return await _context.Trips.CountAsync(t => t.TripType == "غير مباشرة");
        }
        public async Task<bool> DeleteTripAsync(int id)
        {
            try
            {
                var trip = await _context.Trips.FindAsync(id);
                if (trip == null)
                    return false;

                // Soft delete - just deactivate
                trip.IsActive = false;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log error
                Console.WriteLine($"Error deleting trip: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> HardDeleteTripAsync(int id)
        {
            try
            {
                var trip = await _context.Trips.FindAsync(id);
                if (trip == null)
                    return false;

                // Hard delete - remove from database
                _context.Trips.Remove(trip);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log error
                Console.WriteLine($"Error hard deleting trip: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateTripAsync(Trip trip)
        {
            try
            {
                var existingTrip = await _context.Trips.FindAsync(trip.Id);
                if (existingTrip == null)
                    return false;

                existingTrip.TripName = trip.TripName;
                existingTrip.TripNumber = trip.TripNumber;
                existingTrip.DepartureDate = trip.DepartureDate;
                existingTrip.TripType = trip.TripType;
                existingTrip.TotalSeats = trip.TotalSeats;
                existingTrip.FilledSeats = trip.FilledSeats;
                existingTrip.Airline = trip.Airline;
                existingTrip.DepartureFrom = trip.DepartureFrom;
                existingTrip.ArrivalTo = trip.ArrivalTo;
                existingTrip.Program = trip.Program;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating trip: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> CreateAsync(Trip trip)
        {
            try
            {
                _context.Trips.Add(trip);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating trip: {ex.Message}");
                return false;
            }
        }
    }
}
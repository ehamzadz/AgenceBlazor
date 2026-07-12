// Services/PaymentService.cs
using AgenceBlazor.Data;
using AgenceBlazor.Models;
using Microsoft.EntityFrameworkCore;

namespace AgenceBlazor.Services
{
    public class PaymentService
    {
        private readonly ApplicationDbContext _context;

        public PaymentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Payment>> GetAllAsync()
        {
            return await _context.Payments
                .Include(p => p.Agency)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        public async Task<List<Payment>> GetByAgencyIdAsync(Guid agencyId)
        {
            return await _context.Payments
                .Where(p => p.AgencyId == agencyId)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        public async Task<bool> CreateAsync(Payment payment)
        {
            try
            {
                payment.CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
                payment.UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
                payment.PaymentDate = DateTime.SpecifyKind(payment.PaymentDate, DateTimeKind.Utc);

                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving payment: {ex.Message}");
                return false;
            }
        }


        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var payment = await _context.Payments.FindAsync(id);
                if (payment == null) return false;

                _context.Payments.Remove(payment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting payment: {ex.Message}");
                return false;
            }
        }
    }
}
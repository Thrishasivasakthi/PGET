using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace FastX.DAL.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _context;

        public PaymentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Payment> ProcessPaymentAsync(Payment payment)
        {
            // Simulate payment logic
            payment.PaymentStatus = "Success"; // In real world, integrate RazorPay/Stripe/etc.
            payment.PaymentDate = DateTime.UtcNow;

            _context.Payments.Add(payment);

            // Mark booking as paid
            var booking = await _context.Bookings.FindAsync(payment.BookingId);
            if (booking != null)
            {
                booking.Status = "Booked";
            }

            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<Payment> GetPaymentByBookingIdAsync(int bookingId)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(p => p.BookingId == bookingId);
        }

        public async Task<bool> RefundPaymentAsync(int bookingId, decimal amount)
        {
            var payment = await _context.Payments
                .FirstOrDefaultAsync(p => p.BookingId == bookingId);

            if (payment == null || payment.PaymentStatus == "Refunded")
                return false;

            payment.PaymentStatus = "Refunded";
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

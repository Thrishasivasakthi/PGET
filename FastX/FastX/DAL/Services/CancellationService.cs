using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace FastX.DAL.Services
{
    public class CancellationService : ICancellationService
    {
        private readonly ApplicationDbContext _context;

        public CancellationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Cancellation> RecordCancellationAsync(int bookingId, string reason, decimal refundAmount)
        {
            var booking = await _context.Bookings
                .Include(b => b.Payment)
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null || booking.Status == "Cancelled")
                throw new Exception("Booking not found or already cancelled.");

            // Update booking status
            booking.Status = "Cancelled";

            // Update payment status
            if (booking.Payment != null)
            {
                booking.Payment.PaymentStatus = "Refunded";
            }

            // Create cancellation record
            var cancellation = new Cancellation
            {
                BookingId = bookingId,
                Reason = reason,
                CancelledOn = DateTime.UtcNow,
                RefundAmount = refundAmount
            };

            _context.Cancellations.Add(cancellation);
            await _context.SaveChangesAsync();

            return cancellation;
        }

        public async Task<Cancellation> GetCancellationDetailsAsync(int bookingId)
        {
            return await _context.Cancellations
                .Include(c => c.Booking)
                .FirstOrDefaultAsync(c => c.BookingId == bookingId);
        }
    }
}

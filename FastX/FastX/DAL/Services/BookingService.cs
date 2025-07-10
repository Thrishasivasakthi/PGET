using DAL.Interfaces;
using DAL.Models;

using Microsoft.EntityFrameworkCore;

namespace FastX.DAL.Services
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _context;

        public BookingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Booking> BookTicketAsync(int userId, int busId, List<string> seatNumbers)
        {
            // Fetch the bus and route
            var bus = await _context.Buses
                .Include(b => b.Route)
                .Include(b => b.Seats)
                .FirstOrDefaultAsync(b => b.Id == busId);

            if (bus == null)
                throw new Exception("Bus not found");

            // Check seat availability
            foreach (var seat in seatNumbers)
            {
                var seatEntity = bus.Seats.FirstOrDefault(s => s.SeatNumber == seat);
                if (seatEntity == null || seatEntity.IsBooked)
                    throw new Exception($"Seat {seat} is not available");
            }

            // Book the seats
            foreach (var seat in seatNumbers)
            {
                var seatEntity = bus.Seats.First(s => s.SeatNumber == seat);
                seatEntity.IsBooked = true;
            }

            // Calculate total amount
            decimal totalAmount = seatNumbers.Count * bus.Route.Fare;

            var booking = new Booking
            {
                UserId = userId,
                BusId = busId,
                SeatNumbers = string.Join(",", seatNumbers),
                BookingDate = DateTime.UtcNow,
                TotalAmount = totalAmount,
                Status = "Booked"
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return booking;
        }

        public async Task<Booking> GetBookingByIdAsync(int bookingId)
        {
            return await _context.Bookings
                .Include(b => b.Bus)
                .Include(b => b.Bus.Route)
                .FirstOrDefaultAsync(b => b.Id == bookingId);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByUserAsync(int userId)
        {
            return await _context.Bookings
                .Include(b => b.Bus)
                .Include(b => b.Bus.Route)
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> CancelBookingAsync(int bookingId, string reason)
        {
            var booking = await _context.Bookings
                .Include(b => b.Bus)
                .ThenInclude(b => b.Seats)
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null || booking.Status == "Cancelled")
                return false;

            // Mark status
            booking.Status = "Cancelled";

            // Free up seats
            var seatsToCancel = booking.SeatNumbers.Split(',');
            foreach (var seat in seatsToCancel)
            {
                var seatEntity = await _context.Seats
                    .FirstOrDefaultAsync(s => s.BusId == booking.BusId && s.SeatNumber == seat);
                if (seatEntity != null)
                    seatEntity.IsBooked = false;
            }

            // Add cancellation record
            var cancellation = new Cancellation
            {
                BookingId = bookingId,
                Reason = reason,
                CancelledOn = DateTime.UtcNow,
                RefundAmount = booking.TotalAmount // assume full refund
            };

            _context.Cancellations.Add(cancellation);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

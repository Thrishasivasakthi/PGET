using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace FastX.DAL.Services
{
    public class BusOperatorService : IBusOperatorService
    {
        private readonly ApplicationDbContext _context;

        public BusOperatorService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BusOperator> GetProfileAsync(int userId)
        {
            return await _context.BusOperators
                .Include(o => o.Buses)
                .FirstOrDefaultAsync(o => o.UserId == userId);
        }

        public async Task<IEnumerable<Bus>> GetOperatorBusesAsync(int operatorId)
        {
            return await _context.Buses
                .Where(b => b.BusOperatorId == operatorId)
                .Include(b => b.Route)
                .Include(b => b.Seats)
                .ToListAsync();
        }

        public async Task AddBusAsync(Bus bus)
        {
            bus.Seats = new List<Seat>();
            for (int i = 1; i <= bus.TotalSeats; i++)
            {
                bus.Seats.Add(new Seat
                {
                    SeatNumber = $"S{i}",
                    IsBooked = false,
                    BusId = bus.Id
                });
            }

            _context.Buses.Add(bus);
            await _context.SaveChangesAsync();
        }

        public async Task EditBusAsync(Bus updatedBus)
        {
            var bus = await _context.Buses.FindAsync(updatedBus.Id);
            if (bus == null) throw new Exception("Bus not found");

            bus.BusName = updatedBus.BusName;
            bus.BusNumber = updatedBus.BusNumber;
            bus.BusType = updatedBus.BusType;
            bus.TotalSeats = updatedBus.TotalSeats;
            bus.RouteId = updatedBus.RouteId;

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Booking>> GetOperatorBookingsAsync(int operatorId)
        {
            return await _context.Bookings
                .Include(b => b.Bus)
                .ThenInclude(b => b.BusOperator)
                .Include(b => b.User)
                .Where(b => b.Bus.BusOperatorId == operatorId)
                .ToListAsync();
        }

        public async Task<bool> RefundBookingAsync(int bookingId)
        {
            var booking = await _context.Bookings
                .Include(b => b.Payment)
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null || booking.Status == "Refunded")
                return false;

            booking.Status = "Refunded";

            var payment = booking.Payment;
            if (payment != null)
                payment.PaymentStatus = "Refunded";

            await _context.SaveChangesAsync();
            return true;
        }
    }
}

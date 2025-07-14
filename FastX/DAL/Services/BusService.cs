using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace FastX.DAL.Services
{
    public class BusService : IBusService
    {
        private readonly ApplicationDbContext _context;

        public BusService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Bus> GetBusByIdAsync(int id)
        {
            return await _context.Buses
                .Include(b => b.Route)
                .Include(b => b.Seats)
                .Include(b => b.BusAmenities)
                    .ThenInclude(ba => ba.Amenity)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Bus>> GetAvailableBusesAsync(int routeId, DateTime date)
        {
            return await _context.Buses
                .Include(b => b.Route)
                .Include(b => b.Seats)
                .Where(b => b.RouteId == routeId &&
                            b.Route.DepartureTime.Date == date.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Seat>> GetAvailableSeatsAsync(int busId)
        {
            return await _context.Seats
                .Where(s => s.BusId == busId)
                .ToListAsync();
        }

        public async Task AddBusAsync(Bus bus)
        {
            // Ensure seats collection is populated
            bus.Seats = new List<Seat>();
            for (int i = 1; i <= bus.TotalSeats; i++)
            {
                bus.Seats.Add(new Seat
                {
                    SeatNumber = $"S{i}",
                    IsBooked = false,
                    BusId = bus.Id // if using identity insert; EF will link on Save
                });
            }

            _context.Buses.Add(bus);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBusAsync(Bus updatedBus)
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
    }
}

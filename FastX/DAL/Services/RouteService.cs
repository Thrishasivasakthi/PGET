using Microsoft.EntityFrameworkCore;
using DAL.Interfaces;
using DAL.Models;

namespace FastX.DAL.Services
{
    public class RouteService : IRouteService
    {
        private readonly ApplicationDbContext _context;

        public RouteService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Route>> SearchRoutesAsync(string origin, string destination, DateTime date)
        {
            // Clean inputs for search (case-insensitive)
            origin = origin.Trim().ToLower();
            destination = destination.Trim().ToLower();

            return await _context.Routes
                .Include(r => r.Buses)
                .Where(r => r.Origin.ToLower() == origin &&
                            r.Destination.ToLower() == destination &&
                            r.DepartureTime.Date == date.Date)
                .ToListAsync();
        }

        public async Task<Route> AddRouteAsync(Route route)
        {
            // Validate route (optional: prevent duplicate)
            _context.Routes.Add(route);
            await _context.SaveChangesAsync();
            return route;
        }

        public async Task<Route> GetRouteByIdAsync(int id)
        {
            return await _context.Routes
                .Include(r => r.Buses)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Route>> GetAllRoutesAsync()
        {
            return await _context.Routes
                .Include(r => r.Buses)
                .ToListAsync();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using DAL.Interfaces;
using DAL.Models;

namespace FastX.DAL.Services
{
    public class AmenityService : IAmenityService
    {
        private readonly ApplicationDbContext _context;

        public AmenityService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Amenity>> GetAllAmenitiesAsync()
        {
            return await _context.Amenities.ToListAsync();
        }

        public async Task<IEnumerable<Amenity>> GetAmenitiesByBusIdAsync(int busId)
        {
            return await _context.BusAmenities
                .Include(ba => ba.Amenity)
                .Where(ba => ba.BusId == busId)
                .Select(ba => ba.Amenity)
                .ToListAsync();
        }

        public async Task AssignAmenitiesToBusAsync(int busId, List<int> amenityIds)
        {
            // Remove existing amenities first
            var existing = await _context.BusAmenities
                .Where(ba => ba.BusId == busId)
                .ToListAsync();

            _context.BusAmenities.RemoveRange(existing);

            // Assign new
            var newAmenities = amenityIds.Select(aid => new BusAmenity
            {
                BusId = busId,
                AmenityId = aid
            });

            await _context.BusAmenities.AddRangeAsync(newAmenities);
            await _context.SaveChangesAsync();
        }
    }
}

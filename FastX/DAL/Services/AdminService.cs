using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace FastX.DAL.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;

        public AdminService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .Where(u => u.Role == "User")
                .ToListAsync();
        }

        public async Task<IEnumerable<BusOperator>> GetAllOperatorsAsync()
        {
            return await _context.BusOperators
                //.Include(o => o.User)
                .ToListAsync();
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null || user.Role != "User") return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteOperatorAsync(int operatorId)
        {
            var operatorProfile = await _context.BusOperators
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == operatorId);

            if (operatorProfile == null) return false;

            _context.BusOperators.Remove(operatorProfile);

            // Also remove linked user
            if (operatorProfile.User != null)
                _context.Users.Remove(operatorProfile.User);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _context.Bookings
                .Include(b => b.Bus)
                .ThenInclude(b => b.Route)
                .Include(b => b.User)
                .ToListAsync();
        }
    }
}

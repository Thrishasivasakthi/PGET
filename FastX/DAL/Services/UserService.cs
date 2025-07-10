using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using DAL.Interfaces;
using DAL.Models;

namespace FastX.DAL.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
       

        public UserService(ApplicationDbContext context)
        {
            _context = context;
            
        }


        public async Task<User> AuthenticateAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || !VerifyPassword(password, user.PasswordHash))
                return null;
            return user;

          
        }



        public async Task<User> RegisterAsync(User user)
        {
            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
                throw new Exception("Email already registered.");

            user.PasswordHash = HashPassword(user.PasswordHash);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        


        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> UpdateProfileAsync(User updatedUser)
        {
            var user = await _context.Users.FindAsync(updatedUser.Id);
            if (user == null)
                throw new Exception("User not found");

            // Allow updating name, email, maybe password
            user.FullName = updatedUser.FullName;
            user.Email = updatedUser.Email;

            if (!string.IsNullOrWhiteSpace(updatedUser.PasswordHash))
            {
                user.PasswordHash = HashPassword(updatedUser.PasswordHash);
            }

            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<IEnumerable<Booking>> GetUserBookingsAsync(int userId)
        {
            return await _context.Bookings
                .Include(b => b.Bus)
                .Include(b => b.Bus.Route)
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> CancelBookingAsync(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking == null || booking.Status == "Cancelled")
                return false;

            booking.Status = "Cancelled";
            await _context.SaveChangesAsync();
            return true;
        }

        private string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password));

            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private bool VerifyPassword(string input, string hash)
        {
            return HashPassword(input) == hash;
        }
    }
}

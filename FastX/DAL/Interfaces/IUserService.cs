using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IUserService
    {
        Task<User> RegisterAsync(User user);
        Task<User> AuthenticateAsync(string email, string password);
        Task<User> GetByIdAsync(int id);
        Task<User> UpdateProfileAsync(User user); // Add this

        Task<IEnumerable<Booking>> GetUserBookingsAsync(int userId);
        Task<bool> CancelBookingAsync(int bookingId);
    }

}

using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IBookingService
    {
        Task<Booking> BookTicketAsync(int userId, int busId, List<string> seatNumbers);
        Task<Booking> GetBookingByIdAsync(int bookingId);
        Task<IEnumerable<Booking>> GetBookingsByUserAsync(int userId);
        Task<bool> CancelBookingAsync(int bookingId, string reason);

        

    }

}

using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IBusOperatorService
    {
        Task<BusOperator> GetProfileAsync(int userId);
        Task<IEnumerable<Bus>> GetOperatorBusesAsync(int operatorId);
        Task AddBusAsync(Bus bus);
        Task EditBusAsync(Bus bus);
        Task<IEnumerable<Booking>> GetOperatorBookingsAsync(int operatorId);
        Task<bool> RefundBookingAsync(int bookingId);
    }

}

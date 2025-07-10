using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<BusOperator>> GetAllOperatorsAsync();
        Task<bool> DeleteUserAsync(int userId);
        Task<bool> DeleteOperatorAsync(int operatorId);
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
    }

}

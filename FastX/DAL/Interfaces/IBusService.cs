using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IBusService
    {
        Task<Bus> GetBusByIdAsync(int id);
        Task<IEnumerable<Bus>> GetAvailableBusesAsync(int routeId, DateTime date);
        Task<IEnumerable<Seat>> GetAvailableSeatsAsync(int busId);
        Task AddBusAsync(Bus bus);
        Task UpdateBusAsync(Bus bus);
    }

}

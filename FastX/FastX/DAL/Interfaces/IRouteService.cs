using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IRouteService
    {
        Task<IEnumerable<Route>> SearchRoutesAsync(string origin, string destination, DateTime date);
        Task<Route> AddRouteAsync(Route route);
        Task<Route> GetRouteByIdAsync(int id);
        Task<IEnumerable<Route>> GetAllRoutesAsync();


    }

}

using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IAmenityService
    {
        Task<IEnumerable<Amenity>> GetAllAmenitiesAsync();
        Task<IEnumerable<Amenity>> GetAmenitiesByBusIdAsync(int busId);
        Task AssignAmenitiesToBusAsync(int busId, List<int> amenityIds);
    }

}

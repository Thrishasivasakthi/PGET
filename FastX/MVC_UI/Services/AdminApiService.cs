using DAL.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace MVC_UI.Services
{
    public class AdminApiService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IHttpContextAccessor _contextAccessor;

        private string Token => _contextAccessor.HttpContext?.Session.GetString("JWToken") ?? "";

        public AdminApiService(IHttpClientFactory clientFactory, IHttpContextAccessor contextAccessor)
        {
            _clientFactory = clientFactory;
            _contextAccessor = contextAccessor;
        }

        private HttpClient CreateAuthorizedClient()
        {
            var client = _clientFactory.CreateClient();
            if (!string.IsNullOrEmpty(Token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            }
            else
            {
                Console.WriteLine("JWT Token missing from session.");
            }
            return client;
        }



        // AdminController 

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var client = CreateAuthorizedClient();
            var res = await client.GetAsync("http://localhost:5202/api/admin/users");
            if (!res.IsSuccessStatusCode) return Enumerable.Empty<User>();
            return JsonConvert.DeserializeObject<IEnumerable<User>>(await res.Content.ReadAsStringAsync());
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var client = CreateAuthorizedClient();
            var res = await client.DeleteAsync($"http://localhost:5202/api/admin/user/{userId}");
            return res.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<BusOperator>> GetAllOperatorsAsync()
        {
            try
            {
                var client = CreateAuthorizedClient();
                var res = await client.GetAsync("http://localhost:5202/api/admin/operators");

                if (!res.IsSuccessStatusCode)
                {
                    // log or debug
                    return new List<BusOperator>();
                }

                var json = await res.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<BusOperator>>(json) ?? new List<BusOperator>();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error fetching operators: " + ex.Message);
                return new List<BusOperator>(); // fallback to avoid null
            }
        }

        

        public async Task<bool> DeleteOperatorAsync(int operatorId)
        {
            var client = CreateAuthorizedClient();
            var res = await client.DeleteAsync($"http://localhost:5202/api/admin/operator/{operatorId}");
            return res.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            var client = CreateAuthorizedClient();
            var res = await client.GetAsync("http://localhost:5202/api/admin/bookings");
            if (!res.IsSuccessStatusCode) return Enumerable.Empty<Booking>();
            return JsonConvert.DeserializeObject<IEnumerable<Booking>>(await res.Content.ReadAsStringAsync());
        }

        public async Task<bool> AddRouteAsync(DAL.Models.Route route)
        {
            var client = CreateAuthorizedClient();
            var res = await client.PostAsJsonAsync("http://localhost:5202/api/admin/add-route", route);
            return res.IsSuccessStatusCode;
        }


        // AmenityController

        public async Task<List<Amenity>> GetAllAmenitiesAsync()
        {
            var client = CreateAuthorizedClient();
            var res = await client.GetAsync("http://localhost:5202/api/amenity/all");

            if (!res.IsSuccessStatusCode)
                return new List<Amenity>();

            // Fix: Deserialize only the `$values` array
            var raw = await res.Content.ReadAsStringAsync();
            var parsed = JObject.Parse(raw); // Needs: using Newtonsoft.Json.Linq;
            var values = parsed["$values"];

            return values != null
                ? values.ToObject<List<Amenity>>()
                : new List<Amenity>();
        }


        public async Task<IEnumerable<Amenity>> GetAmenitiesByBusAsync(int busId)
        {
            var client = CreateAuthorizedClient();
            var res = await client.GetAsync($"http://localhost:5202/api/amenity/bus/{busId}");
            if (!res.IsSuccessStatusCode) return Enumerable.Empty<Amenity>();
            return JsonConvert.DeserializeObject<IEnumerable<Amenity>>(await res.Content.ReadAsStringAsync());
        }

        public async Task<bool> AssignAmenitiesToBusAsync(int busId, List<int> amenityIds)
        {
            var client = CreateAuthorizedClient();
            var res = await client.PostAsJsonAsync($"http://localhost:5202/api/amenity/assign?busId={busId}", amenityIds);
            return res.IsSuccessStatusCode;
        }

        
        // BusController
        
        public async Task<Bus> GetBusByIdAsync(int busId)
        {
            var client = CreateAuthorizedClient();
            var res = await client.GetAsync($"http://localhost:5202/api/bus/{busId}");
            if (!res.IsSuccessStatusCode) return null;
            return JsonConvert.DeserializeObject<Bus>(await res.Content.ReadAsStringAsync());
        }

        public async Task<IEnumerable<Bus>> GetAvailableBusesAsync(int routeId, DateTime date)
        {
            var client = CreateAuthorizedClient();
            var res = await client.GetAsync($"http://localhost:5202/api/bus/route/{routeId}/date/{date:yyyy-MM-dd}");
            if (!res.IsSuccessStatusCode) return Enumerable.Empty<Bus>();
            return JsonConvert.DeserializeObject<IEnumerable<Bus>>(await res.Content.ReadAsStringAsync());
        }

        public async Task<IEnumerable<string>> GetAvailableSeatsAsync(int busId)
        {
            var client = CreateAuthorizedClient();
            var res = await client.GetAsync($"http://localhost:5202/api/bus/seats/{busId}");
            if (!res.IsSuccessStatusCode) return Enumerable.Empty<string>();

            var seats = JsonConvert.DeserializeObject<IEnumerable<Seat>>(await res.Content.ReadAsStringAsync());
            if (seats == null) return Enumerable.Empty<string>(); // Handle possible null reference

            return seats.Select(seat => seat.SeatNumber); // Explicitly convert Seat objects to strings
        }

        public async Task<bool> AddBusAsync(Bus bus)
        {
            var client = CreateAuthorizedClient();
            var res = await client.PostAsJsonAsync("http://localhost:5202/api/bus/add", bus);
            return res.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateBusAsync(Bus bus)
        {
            var client = CreateAuthorizedClient();
            var res = await client.PutAsJsonAsync("http://localhost:5202/api/bus/update", bus);
            return res.IsSuccessStatusCode;
        }


        // BusOperatorController
      
        public async Task<BusOperator> GetOperatorProfileAsync(int userId)
        {
            var client = CreateAuthorizedClient();
            var res = await client.GetAsync($"http://localhost:5202/api/busoperator/profile/{userId}");
            if (!res.IsSuccessStatusCode) return null;
            return JsonConvert.DeserializeObject<BusOperator>(await res.Content.ReadAsStringAsync());
        }

        public async Task<IEnumerable<Bus>> GetOperatorBusesAsync(int operatorId)
        {
            var client = CreateAuthorizedClient();
            var res = await client.GetAsync($"http://localhost:5202/api/busoperator/buses/{operatorId}");
            if (!res.IsSuccessStatusCode) return Enumerable.Empty<Bus>();
            return JsonConvert.DeserializeObject<IEnumerable<Bus>>(await res.Content.ReadAsStringAsync());
        }

        public async Task<IEnumerable<Booking>> GetOperatorBookingsAsync(int operatorId)
        {
            var client = CreateAuthorizedClient();
            var res = await client.GetAsync($"http://localhost:5202/api/busoperator/bookings/{operatorId}");
            if (!res.IsSuccessStatusCode) return Enumerable.Empty<Booking>();
            return JsonConvert.DeserializeObject<IEnumerable<Booking>>(await res.Content.ReadAsStringAsync());
        }

        public async Task<bool> AddBusForOperatorAsync(Bus bus)
        {
            var client = CreateAuthorizedClient();
            var res = await client.PostAsJsonAsync("http://localhost:5202/api/busoperator/add-bus", bus);
            return res.IsSuccessStatusCode;
        }

        public async Task<bool> EditBusForOperatorAsync(Bus bus)
        {
            var client = CreateAuthorizedClient();
            var res = await client.PutAsJsonAsync("http://localhost:5202/api/busoperator/edit-bus", bus);
            return res.IsSuccessStatusCode;
        }

        public async Task<bool> RefundBookingAsync(int bookingId)
        {
            var client = CreateAuthorizedClient();
            var res = await client.PutAsync($"http://localhost:5202/api/busoperator/refund/{bookingId}", null);
            return res.IsSuccessStatusCode;
        }
        public async Task<IEnumerable<DAL.Models.Route>> GetAllRoutesAsync()
        {
            var client = CreateAuthorizedClient();
            var res = await client.GetAsync("http://localhost:5202/api/route/all");
            if (!res.IsSuccessStatusCode) return new List<DAL.Models.Route>();
            return JsonConvert.DeserializeObject<IEnumerable<DAL.Models.Route>>(await res.Content.ReadAsStringAsync());
        }

        

    }
}

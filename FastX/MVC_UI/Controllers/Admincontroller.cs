using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_UI.Models;
using MVC_UI.Services;
using System.Text;

namespace MVC_UI.Controllers
{
    public class AdminController : Controller
    {
        private readonly AdminApiService _apiService;

        public AdminController(AdminApiService apiService)
        {
            _apiService = apiService;
        }

        // Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var users = await _apiService.GetAllUsersAsync();
            var operators = await _apiService.GetAllOperatorsAsync();

            Console.WriteLine($"Operators count: {operators?.Count()}");

            ViewBag.Users = users;
            ViewBag.Operators = operators;

            return View();
        }

        // Bookings
        public async Task<IActionResult> Bookings()
        {
            var bookings = await _apiService.GetAllBookingsAsync();
            return View(bookings);
        }

        // Routes
        [HttpGet]
        public async Task<IActionResult> AddRoute()
        {
            var operators = await _apiService.GetAllOperatorsAsync();
            ViewBag.Operators = new SelectList(operators, "Id", "CompanyName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddRoute(AddRouteRequest routeRequest)
        {
            if (!ModelState.IsValid) return View(routeRequest);

            var route = new DAL.Models.Route
            {
                Origin = routeRequest.Origin,
                Destination = routeRequest.Destination,
                DepartureTime = routeRequest.DepartureTime,
                ArrivalTime = routeRequest.ArrivalTime,
                Fare = routeRequest.Fare
            };

            var success = await _apiService.AddRouteAsync(route);
            if (success) return RedirectToAction("Dashboard");

            ViewBag.Error = "Failed to add route.";
            return View(routeRequest);
        }



        //Users & Operators
        public async Task<IActionResult> DeleteUser(int id)
        {
            var success = await _apiService.DeleteUserAsync(id);
            TempData["Message"] = success ? "User deleted." : "Delete failed.";
            return RedirectToAction("Dashboard");
        }

        public async Task<IActionResult> DeleteOperator(int id)
        {
            var success = await _apiService.DeleteOperatorAsync(id);
            TempData["Message"] = success ? "Operator deleted." : "Delete failed.";
            return RedirectToAction("Dashboard");
        }

        // Amenities
        public async Task<IActionResult> Amenities()
        {
            var amenities = await _apiService.GetAllAmenitiesAsync();
            return View(amenities);
        }

        [HttpGet]
        public async Task<IActionResult> AssignAmenities(int busId)
        {
            ViewBag.BusId = busId;
            ViewBag.AllAmenities = await _apiService.GetAllAmenitiesAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AssignAmenities(int busId, List<int> selectedAmenityIds)
        {
            var success = await _apiService.AssignAmenitiesToBusAsync(busId, selectedAmenityIds);
            return success ? RedirectToAction("Dashboard") : View();
        }

        // Bus Management
        [HttpGet]
        public IActionResult AddBus() => View();

        [HttpPost]
        public async Task<IActionResult> AddBus(Bus bus)
        {
            var success = await _apiService.AddBusAsync(bus);
            return success ? RedirectToAction("Dashboard") : View(bus);
        }

        [HttpGet]
        
        public async Task<IActionResult> EditBus(int id)
        {
            var bus = await _apiService.GetBusByIdAsync(id);
            if (bus == null)
            {
                
                return NotFound();
            }
            var routes = await _apiService.GetAllRoutesAsync();  // ← Add this if not done already

            ViewBag.RouteId = new SelectList(routes, "Id", "Origin"); // Or Destination or custom label

            return View(bus);
        }


        [HttpPost]
        public async Task<IActionResult> EditBus(Bus bus)
        {
            var success = await _apiService.UpdateBusAsync(bus);
            return success ? RedirectToAction("Dashboard") : View(bus);
        }

        public async Task<IActionResult> OperatorBuses()
        {
            var allOperators = await _apiService.GetAllOperatorsAsync();
            var allBuses = new List<Bus>();

            foreach (var op in allOperators)
            {
                var buses = await _apiService.GetOperatorBusesAsync(op.Id);
                if (buses != null)
                    allBuses.AddRange(buses);
            }

            return View("OperatorBuses", allBuses);
        }
        public async Task<IActionResult> AvailableSeats(int busId)
        {
            var seats = await _apiService.GetAvailableSeatsAsync(busId);
            return View(seats);
        }

        public async Task<IActionResult> GetBusById(int id)
        {
            var bus = await _apiService.GetBusByIdAsync(id);
            return View(bus);
        }

        // Operator Details
        public async Task<IActionResult> OperatorProfile()
        {
            var operators = await _apiService.GetAllOperatorsAsync();
            return View(operators);
        }

        public async Task<IActionResult> ViewOperator(int id)
        {
            var profile = await _apiService.GetOperatorProfileAsync(id);
            var buses = await _apiService.GetOperatorBusesAsync(id);
            var bookings = await _apiService.GetOperatorBookingsAsync(id);

            if (profile == null)
            {
                TempData["Error"] = "Operator not found.";
                return RedirectToAction("OperatorProfile");
            }

            var model = new Models.OperatorDetailsViewModel
            {
                Operator = profile,
                Buses = buses,
                Bookings = bookings
            };

            return View(model);
        }

        public async Task<IActionResult> RefundBooking(int bookingId)
        {
            var result = await _apiService.RefundBookingAsync(bookingId);
            TempData["Message"] = result ? "Refund successful." : "Refund failed.";
            return RedirectToAction("Bookings");
        }




        // API to search buses by route and date
        public async Task<IActionResult> BusesByRoute(int routeId, DateTime date)
        {
            var buses = await _apiService.GetAvailableBusesAsync(routeId, date);
            return View(buses);
        }

        public async Task<IActionResult> ExportBookingsCsv()
        {
            var bookings = await _apiService.GetAllBookingsAsync();
            var csv = new StringBuilder();
            csv.AppendLine("BookingId,User,Bus,SeatNumbers,Amount,Status");

            foreach (var b in bookings)
            {
                csv.AppendLine($"{b.Id},{b.User?.FullName},{b.Bus?.BusName},{b.SeatNumbers},{b.TotalAmount},{b.Status}");
            }

            return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", "bookings.csv");
        }

    }
}

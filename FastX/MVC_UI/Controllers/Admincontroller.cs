using DAL.Models;
using Microsoft.AspNetCore.Mvc;
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
            ViewBag.Users = await _apiService.GetAllUsersAsync();
            ViewBag.Operators = await _apiService.GetAllOperatorsAsync();
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
        public IActionResult AddRoute() => View();

        [HttpPost]
        public async Task<IActionResult> AddRoute(DAL.Models.Route route)
        {
            if (!ModelState.IsValid) return View(route);
            var success = await _apiService.AddRouteAsync(route);
            if (success) return RedirectToAction("Dashboard");
            ViewBag.Error = "Failed to add route.";
            return View(route);
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
            return View(bus);
        }

        [HttpPost]
        public async Task<IActionResult> EditBus(Bus bus)
        {
            var success = await _apiService.UpdateBusAsync(bus);
            return success ? RedirectToAction("Dashboard") : View(bus);
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
        public async Task<IActionResult> OperatorProfile(int userId)
        {
            var profile = await _apiService.GetOperatorProfileAsync(userId);
            return View(profile);
        }

        public async Task<IActionResult> OperatorBuses(int operatorId)
        {
            var buses = await _apiService.GetOperatorBusesAsync(operatorId);
            return View(buses);
        }

        public async Task<IActionResult> OperatorBookings(int operatorId)
        {
            var bookings = await _apiService.GetOperatorBookingsAsync(operatorId);
            return View(bookings);
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

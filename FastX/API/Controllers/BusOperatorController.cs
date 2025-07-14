using DAL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [Authorize(Roles = "Admin,Operator")]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BusOperatorController : ControllerBase
    {
        private readonly IBusOperatorService _busOperatorService;
        private readonly ILogger<BusOperatorController> _logger;

        public BusOperatorController(IBusOperatorService busOperatorService, ILogger<BusOperatorController> logger)
        {
            _busOperatorService = busOperatorService;
            _logger = logger;
        }

        // api/busoperator/profile/2
        // This endpoint retrieves the profile of a bus operator by user ID
        [HttpGet("profile/{userId}")]
        public async Task<IActionResult> GetProfile(int userId)
        {
            _logger.LogInformation("Fetching profile for user ID: {UserId}", userId);
            var profile = await _busOperatorService.GetProfileAsync(userId);
            if (profile == null)
            {
                _logger.LogWarning("Profile not found for user ID: {UserId}", userId);
                return NotFound(new { message = "Operator profile not found." });
            }

            _logger.LogInformation("Retrieved profile for user ID: {UserId}", userId);
            return Ok(profile);
        }

        // api/busoperator/buses/3
        // This endpoint retrieves all buses operated by a specific operator
        [HttpGet("buses/{operatorId}")]
        public async Task<IActionResult> GetOperatorBuses(int operatorId)
        {
            _logger.LogInformation("Fetching buses for operator ID: {OperatorId}", operatorId);
            var buses = await _busOperatorService.GetOperatorBusesAsync(operatorId);

            if (buses == null || !buses.Any())
            {
                _logger.LogWarning("No buses found for operator ID: {OperatorId}", operatorId);
                return NotFound(new { message = "No buses found for this operator." });
            }

            _logger.LogInformation("Retrieved {Count} buses for operator ID: {OperatorId}", buses.Count(), operatorId);
            return Ok(buses);
        }

        // api/busoperator/bookings/3
        // This endpoint retrieves all bookings made by a specific operator
        [HttpGet("bookings/{operatorId}")]
        public async Task<IActionResult> GetOperatorBookings(int operatorId)
        {
            _logger.LogInformation("Fetching bookings for operator ID: {OperatorId}", operatorId);
            var bookings = await _busOperatorService.GetOperatorBookingsAsync(operatorId);

            if (bookings == null || !bookings.Any())
            {
                _logger.LogWarning("No bookings found for operator ID: {OperatorId}", operatorId);
                return NotFound(new { message = "No bookings found for this operator." });
            }

            _logger.LogInformation("Retrieved {Count} bookings for operator ID: {OperatorId}", bookings.Count(), operatorId);
            return Ok(bookings);
        }

        // api/busoperator/add-bus
        // This endpoint allows a bus operator to add a new bus
        [HttpPost("add-bus")]
        public async Task<IActionResult> AddBus([FromBody] Bus bus)
        {
            try
            {   
                _logger.LogInformation("Adding new bus with details: {BusDetails}", bus);
                await _busOperatorService.AddBusAsync(bus);

                _logger.LogInformation("Bus added successfully: {BusName}", bus.BusName);
                return Ok(new { message = "Bus added successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding bus: {BusName}", bus.BusName);
                return BadRequest(new { message = ex.Message });
            }
        }

        // api/busoperator/edit-bus
        // This endpoint allows a bus operator to edit an existing bus
        [HttpPut("edit-bus")]
        public async Task<IActionResult> EditBus([FromBody] Bus bus)
        {
            try
            {
                _logger.LogInformation("Editing bus with ID: {BusId}", bus.Id);
                await _busOperatorService.EditBusAsync(bus);
                _logger.LogInformation("Bus updated successfully: {BusName}", bus.BusName);
                return Ok(new { message = "Bus updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating bus: {BusName}", bus.BusName);
                return BadRequest(new { message = ex.Message });
            }
        }

        // api/busoperator/refund/10
        // This endpoint allows a bus operator to refund a booking
        [HttpPut("refund/{bookingId}")]
        public async Task<IActionResult> RefundBooking(int bookingId)
        {
            _logger.LogInformation("Processing refund for booking ID: {BookingId}", bookingId);
            var result = await _busOperatorService.RefundBookingAsync(bookingId);
            if (!result)
            {
                _logger.LogWarning("Refund failed or already processed for booking ID: {BookingId}", bookingId);
                return BadRequest(new { message = "Refund failed or already processed." });
            }

            _logger.LogInformation("Refund issued successfully for booking ID: {BookingId}", bookingId);
            return Ok(new { message = "Refund issued successfully." });
        }
    }
}

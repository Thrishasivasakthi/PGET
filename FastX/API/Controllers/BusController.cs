using DAL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BusController : ControllerBase
    {
        private readonly IBusService _busService;
        private readonly ILogger<BusController> _logger;

        public BusController(IBusService busService, ILogger<BusController> logger)
        {
            _busService = busService;
            _logger = logger;
        }

        // api/bus/{id}
        // this endpoint retrieves a bus by its ID
        [Authorize(Roles = "Admin,Operator")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBusById(int id)
        {
            _logger.LogInformation("Fetching bus with ID: {BusId}", id);
            var bus = await _busService.GetBusByIdAsync(id);
            if (bus == null)
            {
                _logger.LogWarning("Bus with ID: {BusId} not found.", id);
                return NotFound(new { message = "Bus not found." });
            }

            _logger.LogInformation("Retrieved bus with ID: {BusId}.", id);
            return Ok(bus);
        }

        // api/bus/route/5/date/2025-06-25
        // this endpoint retrieves available buses for a specific route and date
        [Authorize(Roles = "Admin,Operator")]
        [HttpGet("route/{routeId}/date/{date}")]
        public async Task<IActionResult> GetAvailableBuses(int routeId, DateTime date)
        {
            _logger.LogInformation("Fetching available buses for route ID: {RouteId} on date: {Date}", routeId, date);
            var buses = await _busService.GetAvailableBusesAsync(routeId, date);
            if (buses == null || !buses.Any())
            {
                _logger.LogWarning("No buses available for route ID: {RouteId} on date: {Date}.", routeId, date);
                return NotFound(new { message = "No buses available." });
            }

            _logger.LogInformation("Retrieved {Count} buses for route ID: {RouteId} on date: {Date}.", buses.Count(), routeId, date);
            return Ok(buses);
        }

        // api/bus/seats/3
        // this endpoint retrieves available seats for a specific bus
        [Authorize(Roles = "Admin,Operator,User")]
        [HttpGet("seats/{busId}")]
        public async Task<IActionResult> GetAvailableSeats(int busId)
        {
            _logger.LogInformation("Fetching available seats for bus ID: {BusId}", busId);
            var seats = await _busService.GetAvailableSeatsAsync(busId);

            if (seats == null || !seats.Any())
            {
                _logger.LogWarning("No available seats for bus ID: {BusId}.", busId);
                return NotFound(new { message = "No available seats." });
            }

            _logger.LogInformation("Retrieved {Count} available seats for bus ID: {BusId}.", seats.Count(), busId);
            return Ok(seats);
        }

        // api/bus/add
        // this endpoint adds a new bus
        [Authorize(Roles = "Admin,Operator")]
        [HttpPost("add")]
        public async Task<IActionResult> AddBus([FromBody] Bus bus)
        {
            try
            {   
                _logger.LogInformation("Adding new bus: {BusName}", bus.BusName);
                await _busService.AddBusAsync(bus);

                _logger.LogInformation("Bus {BusName} added successfully.", bus.BusName);
                return Ok(new { message = "Bus added successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding bus: {BusName}", bus.BusName);
                return BadRequest(new { message = ex.Message });
            }
        }

        // api/bus/update
        // this endpoint updates an existing bus
        [Authorize(Roles = "Admin,Operator")]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateBus([FromBody] Bus bus)
        {
            try
            {
                _logger.LogInformation("Updating bus with ID: {BusId}", bus.Id);
                await _busService.UpdateBusAsync(bus);
                _logger.LogInformation("Bus with ID: {BusId} updated successfully.", bus.Id);
                return Ok(new { message = "Bus updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating bus with ID: {BusId}", bus.Id);
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

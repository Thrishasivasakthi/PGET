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
    public class RouteController : ControllerBase
    {
        private readonly IRouteService _routeService;
        private readonly IBusService _busService;
        private readonly ILogger<RouteController> _logger;

        public RouteController(IRouteService routeService, IBusService busService, ILogger<RouteController> logger)
        {
            _routeService = routeService;
            _busService = busService;
            _logger = logger;
        }

        [Authorize(Roles = "User")]
        [HttpGet("search")]
        public async Task<IActionResult> SearchRoutes([FromQuery] string origin, [FromQuery] string destination, [FromQuery] DateTime date)
        {
            if (string.IsNullOrWhiteSpace(origin) || string.IsNullOrWhiteSpace(destination))
                return BadRequest(new { message = "Origin and Destination are required." });

            var routes = await _routeService.SearchRoutesAsync(origin, destination, date);
            return Ok(routes);
        }

        // api/route/add
        // This endpoint adds a new route
        [Authorize(Roles = "Admin,Operator")]
        [HttpPost("add")]
        public async Task<IActionResult> AddRoute([FromBody] DAL.Models.Route route) // Change type to DAL.Models.Route
        {
            try
            {   
                _logger.LogInformation("Adding new route: {Route}", route);
                var result = await _routeService.AddRouteAsync(route);

                if (result == null)
                {
                    _logger.LogWarning("Failed to add route: {Route}", route);
                    return BadRequest(new { message = "Failed to add route." });
                }

                _logger.LogInformation("Route added successfully: {Route}", result);
                return Ok(result);
            }
            catch (Exception ex)
            {   
                _logger.LogError(ex, "Error adding route: {Route}", route);
                return BadRequest(new { message = ex.Message });
            }
        }

        // api/route/edit
        // This endpoint edits an existing route
        [Authorize(Roles = "Admin,Operator")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllRoutes()
        {
            _logger.LogInformation("Fetching all routes...");
            var routes = await _routeService.GetAllRoutesAsync();

            if (routes == null || !routes.Any())
            {
                _logger.LogWarning("No routes found.");
                return NotFound(new { message = "No routes found." });
            }

            _logger.LogInformation("Retrieved {Count} routes.", routes.Count());
            return Ok(routes);
        }


        // api/route/{id}
        // This endpoint retrieves a route by its ID
        [Authorize(Roles = "Admin,Operator")]
        [HttpGet("bus/{id}")]
        public async Task<IActionResult> GetBusById(int id)
        {
            _logger.LogInformation("Fetching bus with ID: {BusId}", id);
            var bus = await _busService.GetBusByIdAsync(id);
            if (bus == null)
            {
                _logger.LogWarning("Bus with ID: {BusId} not found.", id);
                return NotFound();
            }

            _logger.LogInformation("Retrieved bus with ID: {BusId}.", id);
            return Ok(bus);
        }
    }
}

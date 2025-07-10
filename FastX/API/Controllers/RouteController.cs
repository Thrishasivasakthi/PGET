using DAL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(Roles = "Admin,BusOperator")]
    [ApiController]
    [Route("api/[controller]")]
    public class RouteController : ControllerBase
    {
        private readonly IRouteService _routeService;
        private readonly IBusService _busService;

        public RouteController(IRouteService routeService, IBusService busService)
        {
            _routeService = routeService;
            _busService = busService;
        }

        // api/route/add
        [HttpPost("add")]
        public async Task<IActionResult> AddRoute([FromBody] DAL.Models.Route route) // Change type to DAL.Models.Route
        {
            try
            {
                var result = await _routeService.AddRouteAsync(route); // No change needed here
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAllRoutes()
        {
            var routes = await _routeService.GetAllRoutesAsync();
            return Ok(routes);
        }

        [HttpGet("bus/{id}")]
        public async Task<IActionResult> GetBusById(int id)
        {
            var bus = await _busService.GetBusByIdAsync(id);
            if (bus == null) return NotFound();
            return Ok(bus);
        }
    }
}

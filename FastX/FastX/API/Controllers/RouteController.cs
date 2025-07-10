using DAL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RouteController : ControllerBase
    {
        private readonly IRouteService _routeService;

        public RouteController(IRouteService routeService)
        {
            _routeService = routeService;
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
    }
}

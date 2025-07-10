using DAL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _config;


        public UserController(IUserService userService, IConfiguration config)
        {
            _userService = userService;
            _config = config;

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Dictionary<string, string> loginData)
        {
            if (!loginData.ContainsKey("email") || !loginData.ContainsKey("password"))
                return BadRequest("Email and password are required");

            string email = loginData["email"];
            string password = loginData["password"];

            var user = await _userService.AuthenticateAsync(email, password);

            if (user == null)
            {
                return Unauthorized("Invalid credentials");
            }

            var token = GenerateJwtToken(user);
            return Ok(new { token, role = user.Role });

        }

        [NonAction]
        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
         new Claim(ClaimTypes.Email, user.Email),
         new Claim(ClaimTypes.Role, user.Role)
     };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // POST: api/user/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            try
            {
                var result = await _userService.RegisterAsync(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        // GET: api/user/profile/5
        [HttpGet("profile/{id}")]
        public async Task<IActionResult> GetProfile(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [Authorize]
        // PUT: api/user/profile
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile(User user)
        {
            try
            {
                var updated = await _userService.UpdateProfileAsync(user);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        // GET: api/user/bookings/5
        [HttpGet("bookings/{userId}")]
        public async Task<IActionResult> GetUserBookings(int userId)
        {
            var bookings = await _userService.GetUserBookingsAsync(userId);
            return Ok(bookings);
        }

        [Authorize]
        // PUT: api/user/cancel-booking/10
        [HttpPut("cancel-booking/{bookingId}")]
        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            var success = await _userService.CancelBookingAsync(bookingId);
            if (!success)
                return BadRequest(new { message = "Unable to cancel booking." });

            return Ok(new { message = "Booking cancelled successfully." });
        }
    }
}

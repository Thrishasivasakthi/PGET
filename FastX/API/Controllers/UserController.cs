using DAL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _config;
        private readonly ILogger<UserController> _logger;


        public UserController(IUserService userService, IConfiguration config, ILogger<UserController> logger)
        {
            _userService = userService;
            _config = config;
            _logger = logger;
        }

        // POST: api/user/login
        // This endpoint handles user login and returns a JWT token
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Dictionary<string, string> loginData)
        {
            if (!loginData.ContainsKey("email") || !loginData.ContainsKey("password"))
            {
                _logger.LogWarning("Login attempt failed: Email or password not provided.");
                return BadRequest("Email and password are required");
            }

            string email = loginData["email"];
            string password = loginData["password"];

            var user = await _userService.AuthenticateAsync(email, password);
            _logger.LogInformation("User login attempt for email: {Email}", email);

            if (user == null)
            {
                return Unauthorized("Invalid credentials");
            }

            _logger.LogInformation("User {Email} authenticated successfully.", email);

            var token = GenerateJwtToken(user);
            _logger.LogInformation("JWT token generated for user {Email}.", email);
            return Ok(new { token, role = user.Role, id = user.Id });

        }

        // Generates a JWT token for the authenticated user
        [NonAction]
        private string GenerateJwtToken(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null");
            }
            _logger.LogInformation("Generating JWT token for user: {Email}", user.Email);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
         new Claim(ClaimTypes.Email, user.Email),
         new Claim(ClaimTypes.Role, user.Role)
     };

            _logger.LogInformation("Claims created for user: {Email}", user.Email);
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            _logger.LogInformation("JWT token created for user: {Email}", user.Email);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // POST: api/user/register
        // This endpoint handles user registration
        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            try
            {
                if (user == null)
                {
                    _logger.LogWarning("Registration attempt failed: User data is null.");
                    return BadRequest("User data is required");
                }
                _logger.LogInformation("User registration attempt for email: {Email}", user.Email);
                var result = await _userService.RegisterAsync(user);

                if (result == null)
                {
                    _logger.LogWarning("Registration failed for user: {Email}", user.Email);
                    return BadRequest("Registration failed");
                }
                _logger.LogInformation("User {Email} registered successfully.", user.Email);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for user: {Email}", user.Email);
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        // GET: api/user/profile/5
        // This endpoint retrieves the profile of a user by ID
        [HttpGet("profile/{id}")]
        public async Task<IActionResult> GetProfile(int id)
        {
            _logger.LogInformation("Fetching profile for user ID: {UserId}", id);
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User with ID: {UserId} not found.", id);
                return NotFound();
            }

            _logger.LogInformation("Retrieved profile for user ID: {UserId}.", id);
            return Ok(user);
        }

        [Authorize]
        // PUT: api/user/profile
        // This endpoint updates the profile of the authenticated user
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile(User user)
        {
            try
            {
                if (user == null)
                {
                    _logger.LogWarning("Update profile attempt failed: User data is null.");
                    return BadRequest("User data is required");
                }
                _logger.LogInformation("Updating profile for user ID: {UserId}", user.Id);
                var updated = await _userService.UpdateProfileAsync(user);

                if (updated == null)
                {
                    _logger.LogWarning("Update failed for user ID: {UserId}", user.Id);
                    return BadRequest("Update failed");
                }
                _logger.LogInformation("Profile updated successfully for user ID: {UserId}.", user.Id);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating profile for user ID: {UserId}", user.Id);
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        // GET: api/user/bookings/5
        // This endpoint retrieves all bookings for a user by their ID
        [HttpGet("bookings/{userId}")]
        public async Task<IActionResult> GetUserBookings(int userId)
        {
            _logger.LogInformation("Fetching bookings for user ID: {UserId}", userId);
            var bookings = await _userService.GetUserBookingsAsync(userId);
            if (bookings == null || !bookings.Any())
            {
                _logger.LogWarning("No bookings found for user ID: {UserId}.", userId);
                return NotFound(new { message = "No bookings found." });
            }
            _logger.LogInformation("Retrieved {Count} bookings for user ID: {UserId}.", bookings.Count(), userId);
            return Ok(bookings);
        }

        [Authorize]
        // PUT: api/user/cancel-booking/10
        // This endpoint cancels a booking by its ID
        [HttpPut("cancel-booking/{bookingId}")]
        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            _logger.LogInformation("Attempting to cancel booking with ID: {BookingId}", bookingId);
            var success = await _userService.CancelBookingAsync(bookingId);
            if (!success)
            {
                _logger.LogWarning("Cancellation failed for booking ID: {BookingId}", bookingId);
                return BadRequest(new { message = "Unable to cancel booking." });
            }

            _logger.LogInformation("Booking with ID: {BookingId} cancelled successfully.", bookingId);
            return Ok(new { message = "Booking cancelled successfully." });
        }
    }
}

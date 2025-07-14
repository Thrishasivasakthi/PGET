using DAL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [Authorize(Roles = "User")]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        // api/payment/process
        // this endpoint processes a payment
        [HttpPost("process")]
        public async Task<IActionResult> ProcessPayment([FromBody] Payment payment)
        {
            try
            {   
                _logger.LogInformation("Processing payment for Booking ID: {BookingId}", payment.BookingId);
                var result = await _paymentService.ProcessPaymentAsync(payment);

                _logger.LogInformation("Payment processed successfully for Booking ID: {BookingId}", payment.BookingId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment for Booking ID: {BookingId}", payment.BookingId);
                return BadRequest(new { message = ex.Message });
            }
        }

        // api/payment/booking/3
        // this endpoint retrieves a payment by booking ID
        [HttpGet("booking/{bookingId}")]
        public async Task<IActionResult> GetPaymentByBookingId(int bookingId)
        {
            _logger.LogInformation("Fetching payment for Booking ID: {BookingId}", bookingId);
            var payment = await _paymentService.GetPaymentByBookingIdAsync(bookingId);
            if (payment == null)
            {
                _logger.LogWarning("Payment not found for Booking ID: {BookingId}", bookingId);
                return NotFound(new { message = "Payment not found." });
            }

            _logger.LogInformation("Retrieved payment for Booking ID: {BookingId}", bookingId);
            return Ok(payment);
        }

        // api/payment/refund/3
        // this endpoint processes a refund for a payment
        [HttpPut("refund/{bookingId}")]
        public async Task<IActionResult> RefundPayment(int bookingId, [FromQuery] decimal amount)
        {
            _logger.LogInformation("Processing refund for Booking ID: {BookingId} with amount: {Amount}", bookingId, amount);
            var success = await _paymentService.RefundPaymentAsync(bookingId, amount);
            if (!success)
            {
                _logger.LogWarning("Refund failed or already processed for Booking ID: {BookingId}", bookingId);
                return BadRequest(new { message = "Refund failed or already processed." });
            }

            _logger.LogInformation("Refund processed successfully for Booking ID: {BookingId}", bookingId);

            return Ok(new { message = "Refund processed successfully." });
        }
    }
}

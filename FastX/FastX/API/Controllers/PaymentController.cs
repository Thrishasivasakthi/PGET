using DAL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // api/payment/process
        [HttpPost("process")]
        public async Task<IActionResult> ProcessPayment([FromBody] Payment payment)
        {
            try
            {
                var result = await _paymentService.ProcessPaymentAsync(payment);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // api/payment/booking/3
        [HttpGet("booking/{bookingId}")]
        public async Task<IActionResult> GetPaymentByBookingId(int bookingId)
        {
            var payment = await _paymentService.GetPaymentByBookingIdAsync(bookingId);
            if (payment == null)
                return NotFound(new { message = "Payment not found." });

            return Ok(payment);
        }

        // api/payment/refund/3
        [HttpPut("refund/{bookingId}")]
        public async Task<IActionResult> RefundPayment(int bookingId, [FromQuery] decimal amount)
        {
            var success = await _paymentService.RefundPaymentAsync(bookingId, amount);
            if (!success)
                return BadRequest(new { message = "Refund failed or already processed." });

            return Ok(new { message = "Refund processed successfully." });
        }
    }
}

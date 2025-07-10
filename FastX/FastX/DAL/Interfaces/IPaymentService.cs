using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IPaymentService
    {
        Task<Payment> ProcessPaymentAsync(Payment payment);
        Task<Payment> GetPaymentByBookingIdAsync(int bookingId);
        Task<bool> RefundPaymentAsync(int bookingId, decimal amount);
    }

}

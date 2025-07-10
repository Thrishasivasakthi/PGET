using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface ICancellationService
    {
        Task<Cancellation> RecordCancellationAsync(int bookingId, string reason, decimal refundAmount);
        Task<Cancellation> GetCancellationDetailsAsync(int bookingId);
    }

}

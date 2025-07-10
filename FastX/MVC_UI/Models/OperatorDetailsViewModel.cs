using DAL.Models;

namespace MVC_UI.Models
{
    public class OperatorDetailsViewModel
    {
        public BusOperator Operator { get; set; }
        public IEnumerable<Bus> Buses { get; set; }
        public IEnumerable<Booking> Bookings { get; set; }
    }
}

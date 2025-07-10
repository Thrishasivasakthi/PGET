namespace MVC_UI.Models
{
    public class AddRouteRequest
    {
        // Route info
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public decimal Fare { get; set; }

        // Bus info
        public string BusName { get; set; }
        public string BusNumber { get; set; }
        public int BusOperatorId { get; set; }
        public string BusType { get; set; }
        public int TotalSeats { get; set; }
    }
}

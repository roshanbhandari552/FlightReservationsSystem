namespace FlightReservationSystem.Models
{
    public class Schedule
    {
        public Guid Id { get; set; }  

        public Guid FlightId { get; set; }
        public Flight Flight { get; set; }

        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
}

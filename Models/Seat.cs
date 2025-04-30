namespace FlightReservationSystem.Models
{
    public class Seat
    {
        public Guid Id { get; set; } 

        public string SeatNumber { get; set; }   // e.g., 12A
        public string Class { get; set; }        // Economy, Business, First
        public bool IsBooked { get; set; }

        public Guid AircraftId { get; set; }
        public Aircraft Aircraft { get; set; }
    }
}

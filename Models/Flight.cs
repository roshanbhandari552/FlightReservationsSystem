namespace FlightReservationSystem.Models
{
    public class Flight
    {
        public Guid Id { get; set; }  
        public string FlightNumber { get; set; }

        public Guid OriginAirportId { get; set; }
        public Airport OriginAirport { get; set; }

        public Guid DestinationAirportId { get; set; }
        public Airport DestinationAirport { get; set; }

        public Guid AircraftId { get; set; }
        public Aircraft Aircraft { get; set; }

        public TimeSpan EstimatedDuration { get; set; }
    }
}

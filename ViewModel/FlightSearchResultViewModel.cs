using FlightReservationSystem.Models;

namespace FlightReservationSystem.ViewModel
{
    public class FlightSearchResultViewModel
    {
        public List<Flight> OutboundFlights { get; set; } = new();
        public List<Flight> ReturnFlights { get; set; } = new();
        public bool IsRoundTrip { get; set; }
    }
}

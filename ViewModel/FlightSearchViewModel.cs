using FlightReservationSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace FlightReservationSystem.ViewModel
{
    public class FlightSearchViewModel
    {

        [Required(ErrorMessage = "Please select a departure date.")]
        [DataType(DataType.Date)]
        public DateTime DepartureDate { get; set; }

        [Required(ErrorMessage = "Please select a departure airport.")]
        public string SelectedOriginAirportId { get; set; }

        [Required(ErrorMessage = "Please select a destination airport.")]
        public string SelectedDestinationAirportId { get; set; }

        public List<Flight> MatchingFlights { get; set; } = new();

        public List<SelectListItem> OriginAirports { get; set; } = new();

        public List<SelectListItem> DestinationAirports{ get; set; } = new();
    }
}

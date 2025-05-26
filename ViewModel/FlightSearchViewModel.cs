using FlightReservationSystem.Attributes;
using FlightReservationSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace FlightReservationSystem.ViewModel
{
    [DifferentAirportsAttribute]
    public class FlightSearchViewModel
    {
    
        [Required(ErrorMessage = "Please select a departure date.")]
        [DataType(DataType.Date)]
        public DateTime DepartureDate { get; set; }

        [DataType(DataType.Date)]
        [ReturnDateAfterDeparture("DepartureDate", ErrorMessage = "Return date must be after departure date.")]
        public DateTime? ReturnDate { get; set; }


        [Required(ErrorMessage = "Please select a departure airport.")]
        public string SelectedOriginAirportId { get; set; }

        [Required(ErrorMessage = "Please select a destination airport.")]
        public string SelectedDestinationAirportId { get; set; }

        [Display(Name = "Round Trip")]
        public bool IsRoundTrip { get; set; }

        public List<Flight> ReturnFlights { get; set; } = new();

        public List<Flight> MatchingFlights { get; set; } = new();

        public List<SelectListItem> OriginAirports { get; set; } = new();

        public List<SelectListItem> DestinationAirports{ get; set; } = new();
    }
}

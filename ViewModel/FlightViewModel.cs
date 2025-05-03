using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using FlightReservationSystem.Attributes;

namespace FlightReservationSystem.ViewModel
{
    public class FlightViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string FlightNumber { get; set; }

        [Required(ErrorMessage = "Origin Airport is required.")]
        [Display(Name = "Origin Airport")]
        public Guid OriginAirportId { get; set; }

        [Required(ErrorMessage = "Destination Airport is required.")]
        [Display(Name = "Destination Airport")]
        public Guid DestinationAirportId { get; set; }

        [Required(ErrorMessage = "Aircraft selection is required.")]
        [Display(Name = "Aircraft")]
        public Guid AircraftId { get; set; }

        /* [Required(ErrorMessage = "Estimated duration is required.")]
         [Range(typeof(TimeSpan), "00:10:00", "24:00:00", ErrorMessage = "Duration must be between 10 minutes and 24 hours.")]
         [Display(Name = "Estimated Duration")]*/

        [Required]
        [DurationRangeAttribute("00:10:00", "1.00:00:00", ErrorMessage = "Duration must be between 10 minutes and 24 hours.")]
        public TimeSpan EstimatedDuration { get; set; }

        [Required(ErrorMessage = "Flight date is required.")]
        [DataType(DataType.DateTime)]
        [FutureDate(ErrorMessage = "Flight date and time must be in the future.")]
        public DateTime FlightDateTime { get; set; }


        // These are for dropdown lists
        public List<SelectListItem> Airports { get; set; } = new();
        public List<SelectListItem> Aircrafts { get; set; } = new();
    }
}

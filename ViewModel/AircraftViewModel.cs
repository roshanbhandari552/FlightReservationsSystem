using System.ComponentModel.DataAnnotations;

namespace FlightReservationSystem.ViewModel
{
    public class AircraftViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Model is required.")]
        public string Model { get; set; }

        [Required(ErrorMessage = "Manufacturer is required.")]
        public string Manufacturer { get; set; }

        [Range(1, 1000, ErrorMessage = "Capacity must be between 1 and 1000.")]
        public int Capacity { get; set; }


    }
}

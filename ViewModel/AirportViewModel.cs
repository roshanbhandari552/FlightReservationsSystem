using System.ComponentModel.DataAnnotations;

namespace FlightReservationSystem.ViewModel
{
    public class AirportViewModel
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Airport code is required.")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Code must be between 3 and 10 characters.")]
        [Display(Name = "Airport Code")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Airport name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name cannot exceed 100 characters.")]
        [Display(Name = "Airport Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [StringLength(50, MinimumLength = 3)]
        public string City { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        [StringLength(50)]
        public string Country { get; set; }
    }
}

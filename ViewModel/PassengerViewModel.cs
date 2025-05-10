using FlightReservationSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace FlightReservationSystem.ViewModel
{
    public class PassengerViewModel
    {
        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100, ErrorMessage = "Full name can't exceed 100 characters.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Passport number is required.")]
        [StringLength(20, ErrorMessage = "Passport number can't exceed 20 characters.")]
        public string PassportNumber { get; set; }
    }
}

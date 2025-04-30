using System.ComponentModel.DataAnnotations;

namespace FlightReservationSystem.Models
{
    public class Passenger
    {
        public Guid Id { get; set; }  

        public string FullName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(20)]
        public string PassportNumber { get; set; }

        public Guid BookingId { get; set; }
        public Booking Booking { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace FlightReservationSystem.ViewModel
{
    public class BookingViewModel
    {
        public Guid FlightId { get; set; }
        public string UserId { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }  

        public List<PassengerViewModel> Passengers { get; set; } = new();

        [Required]
        public string PaymentMethod { get; set; }
    }
}

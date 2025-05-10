namespace FlightReservationSystem.Models
{
    public class Booking
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public Guid FlightId { get; set; }
        public Flight Flight { get; set; }

        public DateTime BookingDate { get; set; } = DateTime.UtcNow;

        public string Email { get; set; }

        public string PaymentMethod { get; set; }

        public List<Passenger> Passengers { get; set; } = new();
    }

}

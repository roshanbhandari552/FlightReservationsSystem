using System.ComponentModel.DataAnnotations;

namespace FlightReservationSystem.Models
{
    public class Payment
    {
        public Guid Id { get; set; }

        public Guid BookingId { get; set; }
        public Booking Booking { get; set; }

        public string Amount { get; set; }

        [Required]
        public string PaymentMethod { get; set; }  // "Pay at Airport", "eSewa", "PayPal"

        public string ReferenceNumber { get; set; } // Optional: mobile/transaction ID

        public string Status { get; set; } = "Pending"; // Default to Pending

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    }
}

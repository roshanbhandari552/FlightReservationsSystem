using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FlightReservationSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() { }

        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; } = string.Empty;
    }
}

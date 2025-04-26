using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FlightReservationSystem.ViewModel
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(14, MinimumLength = 6)]
        public string FirstName { get; set; }

        [Required]
        [EmailAddress]
        [Remote(action: "IsEmailAvailable", controller: "Account")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public List<String> Roles { get; set; } 
    }
}

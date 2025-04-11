using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Employee
    {

        public int Id { get; set; }

        [Required]
        [StringLength(14, MinimumLength =6)]  
        public string FirstName {  get; set; }

        [Required]
        [EmailAddress]
        [Remote(action: "IsEmailAvailable", controller:"Account")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

    }
}

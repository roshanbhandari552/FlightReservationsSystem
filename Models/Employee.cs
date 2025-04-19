using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Employee
    {

        public int Id { get; set; }

        public string FirstName {  get; set; }
        public string Email { get; set; }
   
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

    }
}

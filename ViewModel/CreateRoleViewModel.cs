using System.ComponentModel.DataAnnotations;

namespace FlightReservationSystem.ViewModel
{
    public class CreateRoleViewModel
    {

        [Required]
        public String Role {  get; set; }
    }
}

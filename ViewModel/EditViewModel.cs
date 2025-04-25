using System.ComponentModel.DataAnnotations;

namespace FlightReservationSystem.ViewModel
{
    public class EditViewModel
    {
        public EditViewModel()
        {
            Users = new List<string>();
        }
        public string Id { get; set; }

        [Required(ErrorMessage = "Role Name is Required")]
        public string Name { get; set; }

        
       public List<String> Users { get; set; }

      
    }
}

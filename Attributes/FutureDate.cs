using System.ComponentModel.DataAnnotations;

namespace FlightReservationSystem.Attributes
{
    public class FutureDate: ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is DateTime dateTime)
            {
                return dateTime > DateTime.Now;
            }
            return false;
        }
    }
}

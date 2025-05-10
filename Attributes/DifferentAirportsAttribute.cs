using FlightReservationSystem.ViewModel;
using System.ComponentModel.DataAnnotations;

namespace FlightReservationSystem.Attributes
{
    public class DifferentAirportsAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var model = (FlightSearchViewModel)context.ObjectInstance;
            if (model.SelectedOriginAirportId == model.SelectedDestinationAirportId)
            {
                return new ValidationResult("Origin and destination airports must be different.");
            }
            return ValidationResult.Success;
        }
    }
}

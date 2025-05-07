using System.ComponentModel.DataAnnotations;

namespace FlightReservationSystem.Attributes
{
    public class ReturnDateAfterDepartureAttribute: ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public ReturnDateAfterDepartureAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
            ErrorMessage = "Return date must be after the departure date.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var returnDate = value as DateTime?;
            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
            var departureDate = property?.GetValue(validationContext.ObjectInstance) as DateTime?;

            if (returnDate.HasValue && departureDate.HasValue && returnDate <= departureDate)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}

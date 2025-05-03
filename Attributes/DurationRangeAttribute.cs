using System.ComponentModel.DataAnnotations;

namespace FlightReservationSystem.Attributes
{
    public class DurationRangeAttribute: ValidationAttribute
    {
        private readonly TimeSpan _min;
        private readonly TimeSpan _max;

        public DurationRangeAttribute(string min, string max)
        {
            _min = TimeSpan.Parse(min);
            _max = TimeSpan.Parse(max);
        }

        public override bool IsValid(object value)
        {
            if (value == null) return false;

            var time = (TimeSpan)value;
            return time >= _min && time <= _max;
        }
    }
}

namespace FlightReservationSystem.Helper
{
    public class TextCapitalize
    {
        public static string CapitalizeEachWord(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;

            return string.Join(" ",
                input.Split(' ')
                     .Where(w => !string.IsNullOrWhiteSpace(w))
                     .Select(w => char.ToUpper(w[0]) + w.Substring(1).ToLower()));
        }
    }
}

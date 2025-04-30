namespace FlightReservationSystem.Models
{
    public class Aircraft
    {
        public Guid Id { get; set; }  
        public string Model { get; set; }     // e.g., Boeing 737
        public int Capacity { get; set; }
        public string Manufacturer { get; set; }
    }
}

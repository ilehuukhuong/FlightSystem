namespace API.Entities
{
    public class Flight
    {
        public int Id { get; set; }
        public string FlightNo { get; set; }
        public string Departure { get; set; }
        public string Arrival { get; set; }
        public bool Status { get; set; }
    }
}

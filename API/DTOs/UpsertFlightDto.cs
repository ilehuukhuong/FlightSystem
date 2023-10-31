namespace API.DTOs
{
    public class UpsertFlightDto
    {
        public int Id { get; set; }
        public string FlightNo { get; set; }
        public string Departure { get; set; }
        public string Arrival { get; set; }
        public bool Status { get; set; }
    }
}

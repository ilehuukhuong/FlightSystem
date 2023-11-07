namespace API.DTOs
{
    public class DocumentDto
    {
        public int Id { get; set; }
        public string DocumentName { get; set; }
        public int FlightId { get; set; }
        public int UploadedByUserId { get; set; }
        public int ConfigurationId { get; set; }
        public bool Status { get; set; }
        public double Version { get; set; }
        public IFormFile File { get; set; }
    }
}

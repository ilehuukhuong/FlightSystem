namespace API.Entities
{
    public class Document
    {
        public int Id { get; set; }
        public string DocumentName { get; set; }
        public int FlightId { get; set; }
        public Flight Flight { get; set; }
        public DateTime UploadDate { get; set; } = DateTime.Now;
        public int UploadedByUserId { get; set; }
        public AppUser UploadedByUser { get; set; }
        public int ConfigurationId { get; set; }
        public Configuration Configuration { get; set; }
        public bool Status { get; set; }
        public string PathFile { get; set; }
        public double Version { get; set; } 
    }
}

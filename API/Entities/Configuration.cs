 namespace API.Entities
{
    public class Configuration
    {
        public int Id { get; set; }
        public string DocumentType { get; set; }
        public string Note { get; set; }
        public List<ConfigurationPermission> ConfigurationPermissions { get; set; }
    }
}

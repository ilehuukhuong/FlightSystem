namespace API.Entities
{
    public class ConfigurationPermission
    {
        public int ConfigurationId { get; set; }
        public Configuration Configuration { get; set; }
        public int PermissionId { get; set; }
        public Permission Permission { get; set; }
        public bool isRead { get; set; }
        public bool isWrite { get; set; }
    }
}

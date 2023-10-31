namespace API.Entities
{
    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public int CreatorId { get; set; }
        public AppUser Creator { get; set; }
        public DateTime Created { get; set; } 
        public List<PermissionUser> PermissionUsers { get; set; }
        public List<ConfigurationPermission> ConfigurationPermissions { get; set; }
    }
}

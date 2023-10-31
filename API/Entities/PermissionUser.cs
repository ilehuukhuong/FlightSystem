namespace API.Entities
{
    public class PermissionUser
    {
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int PermissionId { get; set; }
        public Permission Permission { get; set; }
    }
}

namespace API.DTOs
{
    public class PermissionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public AppUserDto Creator { get; set; }
        public DateTime Created { get; set; }
        public List<AppUserDto> AppUserDtos { get; set; }
    }
}

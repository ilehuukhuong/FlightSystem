namespace API.DTOs
{
    public class UpdatePermissionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public List<AppUserDto> AppUserDtos { get; set; }
    }
}

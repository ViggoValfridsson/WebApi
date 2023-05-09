namespace WebApi.Models.Dtos;

public class RoleDto
{
    public string RoleName { get; set; } = null!;
    public ICollection<UserDto> Users { get; set; } = new HashSet<UserDto>();
}
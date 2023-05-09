namespace WebApi.Models.Dtos;

public class GroupDto
{
    public int Id { get; set; }
    public string GroupName { get; set; } = null!;
    public ICollection<UserDto> Users { get; set; } = new HashSet<UserDto>();
}

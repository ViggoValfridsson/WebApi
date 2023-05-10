namespace WebApi.Models.Dtos;

public class GroupWithUsersDto
{
    public int Id { get; set; }
    public string GroupName { get; set; } = null!;
    public ICollection<UserWithoutGroupsDto> Users { get; set; } = new HashSet<UserWithoutGroupsDto>();
}

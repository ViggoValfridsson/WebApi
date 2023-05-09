using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using WebApi.Models.Entities;

namespace WebApi.Models.Dtos;

public class UserDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public RoleDto Role { get; set; } = null!;
    public ICollection<GroupDto> Groups { get; set; } = new HashSet<GroupDto>();
}

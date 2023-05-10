using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using WebApi.Models.Dtos;

namespace WebApi.Models.Entities;

public class UserEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Column(TypeName = "nvarchar(100)")]
    public string FirstName { get; set; } = null!;

    [Column(TypeName = "nvarchar(100)")]
    public string LastName { get; set; } = null!;

    [Column(TypeName = "nvarchar(320)")]
    public string Email { get; set; } = null!;
    public int RoleId { get; set; }
    public RoleEntity Role { get; set; } = null!;
    public ICollection<UserGroupsEntity> Groups { get; set; } = new HashSet<UserGroupsEntity>();

    public static implicit operator UserWithGroupsDto?(UserEntity entity)
    {
        if (entity == null)
            return null;

        var dto = new UserWithGroupsDto
        {
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
            Id = entity.Id
        };

        if (entity.Role != null)
            dto.Role = entity.Role.RoleName;

        foreach (var userGroups in entity.Groups)
        {
            if (userGroups.Group.GroupName != null)
                dto.Groups.Add(userGroups.Group.GroupName);
        }

        return dto;
    }

    public static implicit operator UserWithoutGroupsDto?(UserEntity entity)
    {
        if (entity == null)
            return null;

        var dto = new UserWithoutGroupsDto
        {
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
            Id = entity.Id
        };

        if (entity.Role != null)
            dto.Role = entity.Role.RoleName;

        return dto;
    }
}

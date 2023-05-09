using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
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

    public static implicit operator UserDto?(UserEntity entity)
    {
        if (entity == null)
            return null;

        var dto = new UserDto
        {
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
            Role = entity.Role.RoleName,
            Id = entity.Id
        };

        foreach (var userGroups in entity.Groups)
            dto.Groups.Add(userGroups.Group.GroupName);

        return dto;
    }
}

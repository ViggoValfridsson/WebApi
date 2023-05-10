using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using WebApi.Models.Dtos;

namespace WebApi.Models.Entities;

public class GroupEntity
{
    public int Id { get; set; }

    [Column(TypeName = "nvarchar(100)")]
    public string GroupName { get; set; } = null!;
    public ICollection<UserGroupsEntity> Users { get; set; } = new HashSet<UserGroupsEntity>();

    public static implicit operator GroupWithUsersDto?(GroupEntity entity)
    {
        if (entity == null)
            return null;

        var dto = new GroupWithUsersDto
        {
           GroupName = entity.GroupName
        };

        foreach (var userGroup in entity.Users)
            dto.Users.Add(userGroup.User!);

        return dto;
    }

    public static implicit operator GroupWithoutUsersDto?(GroupEntity entity)
    {
        if (entity == null)
            return null;

        var dto = new GroupWithoutUsersDto
        {
            GroupName = entity.GroupName
        };

        return dto;
    }
}

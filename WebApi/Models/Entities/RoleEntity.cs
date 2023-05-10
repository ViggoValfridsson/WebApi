using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using WebApi.Models.Dtos;

namespace WebApi.Models.Entities;

public class RoleEntity
{
    public int Id { get; set; }

    [Column(TypeName = "nvarchar(100)")]
    public string RoleName { get; set; } = null!;
    public ICollection<UserEntity> Users { get; set; } = new HashSet<UserEntity>();

    public static implicit operator RoleDto(RoleEntity entity)
    {
        if (entity == null)
            return null!;

        return new RoleDto
        {
            Id = entity.Id,
            RoleName = entity.RoleName,
        };
    }
}

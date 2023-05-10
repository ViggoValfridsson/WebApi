using System.ComponentModel.DataAnnotations;
using WebApi.Models.Entities;

namespace WebApi.Models.Schemas;

public class GroupOrRoleUpdateSchema
{
    [Required]
    public int Id { get; set; }

    [Required]
    [MinLength(2)]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    public static implicit operator RoleEntity(GroupOrRoleUpdateSchema schema)
    {
        if (schema == null)
            return null!;

        return new RoleEntity
        {
            Id = schema.Id,
            RoleName = schema.Name
        };
    }

    public static implicit operator GroupEntity(GroupOrRoleUpdateSchema schema)
    {
        if (schema == null)
            return null!;

        return new GroupEntity
        {
            Id = schema.Id,
            GroupName = schema.Name
        };
    }
}

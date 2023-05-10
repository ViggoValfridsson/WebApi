using System.ComponentModel.DataAnnotations;
using WebApi.Models.Entities;

namespace WebApi.Models.Schemas;

public class GroupOrRoleCreateSchema
{
    [Required]
    [MinLength(2)]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    public static implicit operator RoleEntity(GroupOrRoleCreateSchema schema)
    {
        if (schema == null)
            return null!;

        return new RoleEntity
        {
            RoleName = schema.Name
        };
    }

    public static implicit operator GroupEntity (GroupOrRoleCreateSchema schema)
    {
        if (schema == null)
            return null!;

        return new GroupEntity
        {
            GroupName = schema.Name
        };
    }
}

using System.ComponentModel.DataAnnotations;
using WebApi.Models.Entities;

namespace WebApi.Models.Schemas;

public class RoleCreateSchema
{
    [Required]
    [MinLength(2)]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    public static implicit operator RoleEntity(RoleCreateSchema schema)
    {
        if (schema == null)
            return null!;

        return new RoleEntity
        {
            RoleName = schema.Name
        };
    }
}

using System.ComponentModel.DataAnnotations;
using WebApi.Models.Entities;

namespace WebApi.Models.Schemas;

public class RoleUpdateSchema
{
    [Required]
    public int Id { get; set; }

    [Required]
    [MinLength(2)]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    public static implicit operator RoleEntity(RoleUpdateSchema schema)
    {
        if (schema == null)
            return null!;

        return new RoleEntity
        {
            Id = schema.Id,
            RoleName = schema.Name
        };
    }
}

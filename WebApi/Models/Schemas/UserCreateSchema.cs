using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApi.Models.Entities;

namespace WebApi.Models.Schemas;

public class UserCreateSchema
{
    [MinLength(2)]
    [RegularExpression(@"^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$")]
    public string FirstName { get; set; } = null!;

    [MinLength(2)]
    [RegularExpression(@"^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$")]
    public string LastName { get; set; } = null!;

    [Required]
    [MinLength(6)]
    [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$")]
    public string Email { get; set; } = null!;
    public int RoleId { get; set; }

    public static implicit operator UserEntity(UserCreateSchema schema)
    {
        if (schema == null)
            return null!;

        return new UserEntity
        {
            FirstName = schema.FirstName,
            LastName = schema.LastName,
            Email = schema.Email,
            RoleId = schema.RoleId,
        };
    }
}

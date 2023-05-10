using System.ComponentModel.DataAnnotations;
using WebApi.Models.Entities;

namespace WebApi.Models.Schemas;

public class UserUpdateSchema
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    [MinLength(2)]
    [MaxLength(100)]
    [RegularExpression(@"^[a-öA-Ö]+(([',. -][a-öA-Ö ])?[a-öA-Ö]*)*$")]
    public string FirstName { get; set; } = null!;

    [Required]
    [MinLength(2)]
    [MaxLength(100)]
    [RegularExpression(@"^[a-öA-Ö]+(([',. -][a-öA-Ö ])?[a-öA-Ö]*)*$")]
    public string LastName { get; set; } = null!;

    [Required]
    [MinLength(6)]
    [MaxLength(320)]
    [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$")]
    public string Email { get; set; } = null!;

    [Required]
    public int RoleId { get; set; }
    public ICollection<int> GroupIds { get; set; } = new HashSet<int>();

    public static implicit operator UserEntity(UserUpdateSchema schema)
    {
        if (schema == null)
            return null!;

        return new UserEntity
        {
            Id = schema.Id,
            FirstName = schema.FirstName,
            LastName = schema.LastName,
            Email = schema.Email.ToLower(),
            RoleId = schema.RoleId,
        };
    }
}

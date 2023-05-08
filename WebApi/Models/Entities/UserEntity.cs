using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    public ICollection<UserGroupEntity> Groups { get; set; } = new HashSet<UserGroupEntity>();
}

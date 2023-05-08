using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models.Entities;

public class RoleEntity
{
    public int Id { get; set; }

    [Column(TypeName = "nvarchar(100)")]
    public string RoleName { get; set; } = null!;
}

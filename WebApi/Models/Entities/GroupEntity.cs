using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models.Entities;

public class GroupEntity
{
    public int Id { get; set; }

    [Column(TypeName = "nvarchar(100)")]
    public string GroupName { get; set; } = null!;
    public ICollection<UserGroupEntity> Users { get; set; } = new HashSet<UserGroupEntity>();
}

using Microsoft.EntityFrameworkCore;

namespace WebApi.Models.Entities;

[PrimaryKey(nameof(UserId), nameof(GroupId))]
public class UserGroupEntity
{
    public Guid UserId { get; set; }
    public UserEntity User { get; set; } = null!;
    public int GroupId { get; set; }
    public GroupEntity Group { get; set; } = null!;
}
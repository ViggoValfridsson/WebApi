using Microsoft.EntityFrameworkCore;
using System.Data;
using WebApi.Models.Entities;

namespace WebApi.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<GroupEntity> Groups { get; set; }
    public DbSet<RoleEntity> Roles { get; set; }
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<UserGroupsEntity> UserGroups { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // To prevent accidental deletion of data it cannot be deleted if it has users.
        modelBuilder.Entity<RoleEntity>()
                  .HasMany(r => r.Users)
                  .WithOne(u => u.Role)
                  .HasForeignKey(u => u.RoleId)
                  .OnDelete(DeleteBehavior.Restrict);

        // No such restriction is necessary for groups/users/groupusers since if a group or user is deleted you also want the value in the join table to be removed
    }
}

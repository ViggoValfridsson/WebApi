using WebApi.Data;
using WebApi.Models.Entities;

namespace WebApi.Helpers.Repositories;

public class RoleRepo : Repo<RoleEntity>
{
    public RoleRepo(DataContext context) : base(context)
    {
    }
}

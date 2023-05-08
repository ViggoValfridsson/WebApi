using WebApi.Data;
using WebApi.Models.Entities;

namespace WebApi.Helpers.Repositories;

public class UserGroupsRepo : Repo<UserGroupsEntity>
{
    public UserGroupsRepo(DataContext context) : base(context)
    {
    }
}

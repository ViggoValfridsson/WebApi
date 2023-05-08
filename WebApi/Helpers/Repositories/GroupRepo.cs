using WebApi.Data;
using WebApi.Models.Entities;

namespace WebApi.Helpers.Repositories;

public class GroupRepo : Repo<GroupEntity>
{
    public GroupRepo(DataContext context) : base(context)
    {
    }
}

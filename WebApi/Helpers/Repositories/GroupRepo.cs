using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Net;
using WebApi.Data;
using WebApi.Models.Entities;
using WebApi.Models.Exceptions;

namespace WebApi.Helpers.Repositories;

public class GroupRepo : Repo<GroupEntity>
{
    private readonly DataContext _context;

    public GroupRepo(DataContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<GroupEntity?> GetAsync(Expression<Func<GroupEntity, bool>> predicate)
    {
        try
        {
            var entity = await _context.Groups.Include(x => x.Users).ThenInclude(x => x.User).ThenInclude(user => user.Role).FirstOrDefaultAsync(predicate);

            return entity;
        }
        catch
        {
            throw new ApiException(HttpStatusCode.BadGateway, "An error occured when fetching the resource. Please try again.");
        }
    }
}

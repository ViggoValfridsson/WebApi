using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Net;
using WebApi.Data;
using WebApi.Models.Entities;
using WebApi.Models.Exceptions;

namespace WebApi.Helpers.Repositories;

public class RoleRepo : Repo<RoleEntity>
{
    private readonly DataContext _context; 

    public RoleRepo(DataContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> AnyAsync(int id)
    {
        try
        {
            return await _context.Roles.AnyAsync(x => x.Id == id);
        }
        catch
        {
            throw new ApiException(HttpStatusCode.BadGateway, "An error occured when fetching the resource. Please try again.");
        }
    }

    public override async Task<RoleEntity?> GetAsync(Expression<Func<RoleEntity, bool>> predicate)
    {
        try
        {
            var entity = await _context.Roles.Include(x => x.Users).FirstOrDefaultAsync(predicate);

            return entity;
        }
        catch
        {
            throw new ApiException(HttpStatusCode.BadGateway, "An error occured when fetching the resource. Please try again.");
        }
    }
}

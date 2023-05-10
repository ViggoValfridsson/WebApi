using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Net;
using WebApi.Data;
using WebApi.Models.Entities;
using WebApi.Models.Exceptions;

namespace WebApi.Helpers.Repositories;

public class UserRepo : Repo<UserEntity>
{
    private readonly DataContext _context;

    public UserRepo(DataContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<UserEntity?> GetAsync(Expression<Func<UserEntity, bool>> predicate)
    {
        try
        {
            var entity = await _context.Users.Include(x => x.Role).Include(x => x.Groups).ThenInclude(x => x.Group).FirstOrDefaultAsync(predicate);

            return entity;
        }
        catch
        {
            throw new ApiException(HttpStatusCode.BadGateway, "An error occured when fetching the resource. Please try again.");
        }
    }

    public override async Task<IEnumerable<UserEntity>> GetAllAsync()
    {
        try
        {
            var entities = await _context.Users.Include(x => x.Role).Include(x => x.Groups).ThenInclude(x => x.Group).ToListAsync();

            return entities;
        }
        catch
        {
            throw new ApiException(HttpStatusCode.BadGateway, "An error occured when fetching the resource. Please try again.");
        }
    }

    public override async Task<IEnumerable<UserEntity>> GetAllAsync(Expression<Func<UserEntity, bool>> predicate)
    {
        try
        {
            var entities = await _context.Users.Include(x => x.Role).Where(predicate).ToListAsync();

            return entities;
        }
        catch
        {
            throw new ApiException(HttpStatusCode.BadGateway, "An error occured when fetching the resource. Please try again.");
        }
    }

    public async Task<bool> AnyAsync(Expression<Func<UserEntity, bool>> predicate)
    {
        return await _context.Users.AnyAsync(predicate);
    }
}

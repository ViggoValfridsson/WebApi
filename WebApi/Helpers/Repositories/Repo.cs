using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Net;
using WebApi.Data;
using WebApi.Models.Exceptions;

namespace WebApi.Helpers.Repositories;

public abstract class Repo<TEntity> where TEntity : class
{
    private readonly DataContext _context;

    protected Repo(DataContext context)
    {
        _context = context;
    }

    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        try
        {
            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
        catch
        {
            throw new ApiException(HttpStatusCode.BadGateway, "An error occured when creating the entity. Please try again.");
        }
    }

    public virtual async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var entity = await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);

            return entity;
        }
        catch
        {
            throw new ApiException(HttpStatusCode.BadGateway, "An error occured when fetching the resource. Please try again.");
        }
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        var entities = await _context.Set<TEntity>().ToListAsync();

        return entities;
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var entities = await _context.Set<TEntity>().Where(predicate).ToListAsync();

            return entities;
        }
        catch
        {
            throw new ApiException(HttpStatusCode.BadGateway, "An error occured when fetching the resource. Please try again.");
        }

    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity)
    {
        try
        {
            _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
        catch
        {
            throw new ApiException(HttpStatusCode.BadGateway, "An error occured when fetching the resource. Please try again.");
        }
    }

    public virtual async Task DeleteAsync(TEntity entity)
    {
        try
        {
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
        }
        catch
        {
            throw new ApiException(HttpStatusCode.BadGateway, "An error occured when deleting the resource. Please try again.");
        }
    }
}

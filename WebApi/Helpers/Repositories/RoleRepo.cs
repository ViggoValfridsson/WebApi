using Microsoft.EntityFrameworkCore;
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
}

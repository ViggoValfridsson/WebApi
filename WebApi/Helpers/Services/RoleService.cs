using System.Linq.Expressions;
using System.Net;
using WebApi.Helpers.Repositories;
using WebApi.Models.Dtos;
using WebApi.Models.Entities;
using WebApi.Models.Exceptions;
using WebApi.Models.Schemas;

namespace WebApi.Helpers.Services;

public class RoleService
{
    private readonly RoleRepo _roleRepo;

    public RoleService(RoleRepo roleRepo)
    {
        _roleRepo = roleRepo;
    }

    public async Task<RoleDto> CreateAsync(RoleCreateSchema schema)
    {
        var entity = await _roleRepo.CreateAsync(schema);

        return entity;
    }

    public async Task<RoleDto?> GetAsync(Expression<Func<RoleEntity, bool>> predicate)
    {
        var entity = await _roleRepo.GetAsync(predicate);

        return entity!;
    }

    public async Task<IEnumerable<RoleDto>> GetAllASync()
    {
        var entities = await _roleRepo.GetAllAsync();

        var dtos = new List<RoleDto>();

        foreach (var entity in entities)
            dtos.Add(entity!);

        return dtos;
    }

    public async Task<RoleDto> UpdateAsync(RoleUpdateSchema schema)
    {
        var entity = await _roleRepo.UpdateAsync(schema);
        return entity;
    }

    public async Task DeleteAsync(Expression<Func<RoleEntity, bool>> predicate)
    {
        var user = await _roleRepo.GetAsync(predicate);

        await _roleRepo.DeleteAsync(user!);
    }
}

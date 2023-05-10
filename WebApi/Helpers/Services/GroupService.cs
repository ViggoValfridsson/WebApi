using System.Linq.Expressions;
using WebApi.Helpers.Repositories;
using WebApi.Models.Dtos;
using WebApi.Models.Entities;
using WebApi.Models.Schemas;

namespace WebApi.Helpers.Services;

public class GroupService
{
    private readonly GroupRepo _groupRepo;

    public GroupService(GroupRepo groupRepo)
    {
        _groupRepo = groupRepo;
    }

    public async Task<GroupWithUsersDto> CreateAsync(GroupOrRoleCreateSchema schema)
    {
        var entity = await _groupRepo.CreateAsync(schema);

        return entity!;
    }

    public async Task<GroupWithUsersDto?> GetAsync(Expression<Func<GroupEntity, bool>> predicate)
    {
        var entity = await _groupRepo.GetAsync(predicate);

        return entity!;
    }

    public async Task<IEnumerable<GroupWithoutUsersDto>> GetAllASync()
    {
        var entities = await _groupRepo.GetAllAsync();
        var dtos = new List<GroupWithoutUsersDto>();

        foreach (var entity in entities)
            dtos.Add(entity!);

        return dtos;
    }

    public async Task UpdateAsync(GroupOrRoleUpdateSchema schema)
    {
        await _groupRepo.UpdateAsync(schema);
    }

    public async Task DeleteAsync(Expression<Func<GroupEntity, bool>> predicate)
    {
        var role = await _groupRepo.GetAsync(predicate);

        await _groupRepo.DeleteAsync(role!);
    }
}

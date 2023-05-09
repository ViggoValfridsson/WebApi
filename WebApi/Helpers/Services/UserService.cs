using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using WebApi.Helpers.Repositories;
using WebApi.Models.Dtos;
using WebApi.Models.Entities;
using WebApi.Models.Exceptions;
using WebApi.Models.Schemas;

namespace WebApi.Helpers.Services;

public class UserService
{
    private readonly UserRepo _userRepo;
    private readonly GroupRepo _groupRepo;
    private readonly RoleRepo _roleRepo;
    private readonly UserGroupsService _userGroupsService;

    public UserService(UserRepo userRepo, GroupRepo groupRepo, RoleRepo roleRepo, UserGroupsRepo userGroupsRepo, UserGroupsService userGroupsService)
    {
        _userRepo = userRepo;
        _groupRepo = groupRepo;
        _roleRepo = roleRepo;
        _userGroupsService = userGroupsService;
    }

    public async Task<UserDto> CreateAsync(UserCreateSchema schema)
    {
        // Checks if role exists 
        if (!(await _roleRepo.AnyAsync(schema.RoleId)))
            throw new ApiException(HttpStatusCode.BadRequest, "The specified role was not found in the database. Make sure that the role id is correct and try again.");

        // Checks if all groupsIds are valid
        foreach (var groupId in schema.GroupIds)
        {
            if (!(await _groupRepo.AnyAsync(groupId)))
                throw new ApiException(HttpStatusCode.BadRequest, "One or more of the specified groups in the request could not be found. Make sure that all group ids are correct and try again.");
        }

        // Checks if email is in use
        if (await GetAsync(x => x.Email == schema.Email.ToLower()) != null)
            throw new ApiException(HttpStatusCode.Conflict, "The specified email address is already in use. Please try again with another email address.");


        var entity = await _userRepo.CreateAsync(schema);

        foreach (var groupId in schema.GroupIds)
            await _userGroupsService.CreateAsync(groupId, entity.Id);

        // Get to get all includes/join data before returning the users
        entity = await _userRepo.GetAsync(x => x.Id == entity.Id);

        return entity!;
    }

    public async Task<UserDto?> GetAsync(Expression<Func<UserEntity, bool>> predicate)
    {
        var entity = await _userRepo.GetAsync(predicate);

        return entity!;
    }

    public async Task<IEnumerable<UserDto>> GetAllASync(Expression<Func<UserEntity, bool>> predicate = null!)
    {
        IEnumerable<UserEntity> entities;

        if (predicate == null)
            entities = await _userRepo.GetAllAsync();
        else
            entities = await _userRepo.GetAllAsync(predicate);

        var dtos = new List<UserDto>();

        foreach (var entity in entities)
            dtos.Add(entity!);

        return dtos;
    }

    public async Task<IEnumerable<UserDto>> GetAllASync(int groupId)
    {
        var group = await _groupRepo.GetAsync(x => x.Id == groupId);

        if (group == null)
            return null!;

        var dtos = new List<UserDto>();

        foreach (var userGroups in group.Users)
            dtos.Add(userGroups.User!);

        return dtos;
    }

    public async Task<UserDto> UpdateAsync(UserUpdateSchema schema)
    {
        try
        {
            var entity = await _userRepo.UpdateAsync(schema);

            return entity!;
        }
        catch (ApiException ex)
        {
            throw ex;
        }
        catch
        {
            throw new ApiException(HttpStatusCode.BadRequest, "Make sure that you entered valid information and try again");
        }

    }

    public async Task DeleteAsync(Guid userId)
    {
        var user = await _userRepo.GetAsync(x => x.Id == userId) ?? throw new ApiException(HttpStatusCode.NotFound, "No entity with that id could be found.");

        await _userRepo.DeleteAsync(user);
    }
}

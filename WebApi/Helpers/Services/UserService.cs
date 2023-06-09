﻿using System.Linq.Expressions;
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
    private readonly UserGroupsRepo _userGroupsRepo;
    private readonly UserGroupsService _userGroupsService;

    public UserService(UserRepo userRepo, GroupRepo groupRepo, UserGroupsRepo userGroupsRepo, UserGroupsService userGroupsService)
    {
        _userRepo = userRepo;
        _groupRepo = groupRepo;
        _userGroupsRepo = userGroupsRepo;
        _userGroupsService = userGroupsService;
    }

    public async Task<bool> AnyAsync(Expression<Func<UserEntity, bool>> predicate)
    {
        return await _userRepo.AnyAsync(predicate);
    }

    public async Task<UserWithGroupsDto> CreateAsync(UserCreateSchema schema)
    {
        var entity = await _userRepo.CreateAsync(schema);

        return entity!;
    }

    public async Task<UserWithGroupsDto?> GetAsync(Expression<Func<UserEntity, bool>> predicate)
    {
        var entity = await _userRepo.GetAsync(predicate);

        return entity!;
    }

    public async Task<IEnumerable<UserWithGroupsDto>> GetAllASync(Expression<Func<UserEntity, bool>> predicate = null!)
    {
        IEnumerable<UserEntity> entities;

        if (predicate == null)
            entities = await _userRepo.GetAllAsync();
        else
            entities = await _userRepo.GetAllAsync(predicate);

        var dtos = new List<UserWithGroupsDto>();

        foreach (var entity in entities)
            dtos.Add(entity!);

        return dtos;
    }

    public async Task<IEnumerable<UserWithGroupsDto>> GetAllASync(int groupId)
    {
        var group = await _groupRepo.GetAsync(x => x.Id == groupId);

        if (group == null)
            return null!;

        var dtos = new List<UserWithGroupsDto>();

        foreach (var userGroups in group.Users)
            dtos.Add(userGroups.User!);

        return dtos;
    }

    public async Task UpdateAsync(UserUpdateSchema schema)
    {
        try
        {
            var currentUserGroups = (await _userGroupsRepo.GetAllAsync(x => x.UserId == schema.Id)).ToList();

            foreach (var currentUserGroup in currentUserGroups)
            {
                if (!schema.GroupIds.Contains(currentUserGroup.GroupId))
                    await _userGroupsRepo.DeleteAsync(currentUserGroup);
            }

            await _userRepo.UpdateAsync(schema);

            // Converts UserGroupEntities to ints for easier comparisons
            var currentGroupIds = new List<int>();

            // fetches group data again in case anything was deleted previously
            currentUserGroups = (await _userGroupsRepo.GetAllAsync(x => x.UserId == schema.Id)).ToList();

            foreach (var group in currentUserGroups)
                currentGroupIds.Add(group.GroupId);

            foreach (var groupId in schema.GroupIds)
            {
                // If user doesn't have the group it is added to the database here
                if (!currentGroupIds.Contains(groupId))
                    await _userGroupsService.CreateAsync(groupId, schema.Id);
            }
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

    public async Task DeleteAsync(Expression<Func<UserEntity, bool>> predicate)
    {
        var user = await _userRepo.GetAsync(predicate);

        await _userRepo.DeleteAsync(user!);
    }
}

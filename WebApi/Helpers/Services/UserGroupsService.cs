using System.Linq.Expressions;
using System.Text.RegularExpressions;
using WebApi.Helpers.Repositories;
using WebApi.Models.Entities;

namespace WebApi.Helpers.Services;

public class UserGroupsService
{
    private readonly UserGroupsRepo _userGroupsRepo;

    public UserGroupsService(UserGroupsRepo userGroupRepo)
    {
        _userGroupsRepo = userGroupRepo;
    }

    public async Task CreateAsync(int groupId, Guid userId)
    {
        await _userGroupsRepo.CreateAsync(new UserGroupsEntity
        {
            GroupId = groupId,
            UserId = userId
        });
    }
}

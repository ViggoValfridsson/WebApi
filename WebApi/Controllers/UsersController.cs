using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Net;
using System.Text.RegularExpressions;
using WebApi.Helpers.Repositories;
using WebApi.Helpers.Services;
using WebApi.Models.Entities;
using WebApi.Models.Exceptions;
using WebApi.Models.Schemas;

namespace WebApi.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;
    private readonly RoleRepo _roleRepo;
    private readonly GroupRepo _groupRepo;
    private readonly UserGroupsService _userGroupsService;

    public UsersController(UserService userService, RoleRepo roleRepo, GroupRepo groupRepo, UserGroupsService userGroupsService)
    {
        _userService = userService;
        _roleRepo = roleRepo;
        _groupRepo = groupRepo;
        _userGroupsService = userGroupsService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            var guid = Guid.Parse(id);
            var user = await _userService.GetAsync(x => x.Id == guid);

            if (user == null)
                return NotFound("Could not find a user with that id");

            return Ok(user);
        }
        catch (FormatException)
        {
            return BadRequest("Invalid id format. The id should consist of 32 hexadecimal digits separated by hyphens.");
        }
        catch (ApiException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.ErrorMessage);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetByEmail(string email)
    {
        try
        {
            if (Regex.IsMatch(email, @"^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$"))
                return BadRequest("Not a valid email address.");

            var user = await _userService.GetAsync(x => x.Email == email.ToLower());

            if (user == null)
                return NotFound("Could not find a user with that email address");

            return Ok(user);
        }
        catch (ApiException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.ErrorMessage);
        }
    }


    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var users = await _userService.GetAllASync();
            return Ok(users);
        }
        catch (ApiException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.ErrorMessage);
        }
    }


    [HttpGet("group")]
    public async Task<IActionResult> GetAllByGroup(int groupId)
    {
        try
        {
            var users = await _userService.GetAllASync(groupId);

            if (users == null)
                return NotFound("Could not find any users. Either the group is empty or it doesn't exist.");

            return Ok(users);
        }
        catch (ApiException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.ErrorMessage);
        }
    }


    [HttpGet("role")]
    public async Task<IActionResult> GetAllByRole(int roleId)
    {
        try
        {
            var users = await _userService.GetAllASync(x => x.RoleId == roleId);

            if (users == null)
                return NotFound("Could not find any users. Either the group is empty or it doesn't exist.");

            return Ok(users);
        }
        catch (ApiException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.ErrorMessage);
        }
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(UserCreateSchema schema)
    {
        if (ModelState.IsValid)
        {
            try
            {
                // Role is required for users therefore we need to make sure that the role is valid.
                if (!(await _roleRepo.AnyAsync(schema.RoleId)))
                    return BadRequest("The specified role was not found in the database. Make sure that the role id is correct and try again.");

                // Check that all groups are valid before creating to avoid creating partial data.
                foreach (var groupId in schema.GroupIds)
                {
                    if (!(await _groupRepo.AnyAsync(groupId)))
                       return BadRequest("One or more of the specified groups in the request could not be found. Make sure that all group ids are correct and try again.");
                }

                if (await _userService.GetAsync(x => x.Email == schema.Email.ToLower()) != null)
                   return Conflict("The specified email address is already in use. Please try again with another email address.");

                // Created here so id is available to create userGroups
                var user = await _userService.CreateAsync(schema);

                foreach (var groupId in schema.GroupIds)
                    await _userGroupsService.CreateAsync(groupId, user.Id);
                
                // Fetch data again so return includes newly added groups
                user = await _userService.GetAsync(x => x.Id == user.Id);

                return Created("", user);
            }
            catch (ApiException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.ErrorMessage);
            }
        }

        return BadRequest("Not a valid schema. Please try again.");
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update(UserUpdateSchema schema)
    {
        if (ModelState.IsValid)
        {
            // kolla ifall nya group listan innehåller nya group id annars radera.
        }

        return BadRequest("Not a valid schema. Please try again.");
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using WebApi.Helpers.Repositories;
using WebApi.Helpers.Services;
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

            if (!(users.Any()))
                return NotFound("No users in the database.");

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

            if (users == null || !(users.Any()))
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

            if (!(users.Any()))
                return NotFound("Could not find any users. Either the role is empty or it doesn't exist.");

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

                if (await _userService.AnyAsync(x => x.Email == schema.Email))
                    return Conflict("The specified email address is already in use. Please try again with another email address.");

                // Created here so id is available later to create userGroups
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

    [HttpPut]
    public async Task<IActionResult> Update(UserUpdateSchema schema)
    {
        if (ModelState.IsValid)
        {
            // Check for valid role to prevent exceptions caused by FK constraints
            if (!(await _roleRepo.AnyAsync(schema.RoleId)))
                return BadRequest("The specified role was not found in the database. Make sure that the role id is correct and try again.");

            // check that roles are valid 
            foreach (var groupId in schema.GroupIds)
            {
                if (!(await _groupRepo.AnyAsync(groupId)))
                    return BadRequest("One or more of the specified groups in the request could not be found. Make sure that all group ids are correct and try again.");
            }

            if (!(await _userService.AnyAsync(x => x.Id == schema.Id)))
                return NotFound("Could not find a user to update. Please make sure your id is valid and try again.");

            await _userService.UpdateAsync(schema);

            // fetch user here to get all updated information as well as all includes
            var user = await _userService.GetAsync(x => x.Id == schema.Id);

            return Ok(user);
        }

        return BadRequest("Not a valid schema. Please try again.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteById(string id)
    {
        try
        {
            var guid = Guid.Parse(id);

            if (!(await _userService.AnyAsync(x => x.Id == guid)))
                return NotFound("Could not find a user to delete. Please make sure your id is valid and try again.");

            await _userService.DeleteAsync(x => x.Id == guid);
            return NoContent();
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


    [HttpDelete]
    public async Task<IActionResult> DeleteByEmail(string email)
    {
        try
        {
            if (!(await _userService.AnyAsync(x => x.Email == email)))
                return NotFound("Could not find a user to delete. Please make sure your id is valid and try again.");

            await _userService.DeleteAsync(x => x.Email == email);

            return NoContent();
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
}

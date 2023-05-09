using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Net;
using WebApi.Helpers.Services;
using WebApi.Models.Entities;
using WebApi.Models.Exceptions;

namespace WebApi.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;

    public UsersController(UserService userService)
    {
        _userService = userService;
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




    //[HttpGet("{groupId}")]
    //public async Task<IActionResult> GetAll(int groupId)
    //{
    //    try
    //    {
    //        var users = await _userService.GetAllASync(groupId);

    //        if (users == null)
    //            return NotFound("Could not find any users. Either the group is empty or it doesn't exist.");

    //        return Ok(users);
    //    }
    //    catch (ApiException ex)
    //    {
    //        return StatusCode((int)ex.StatusCode, ex.ErrorMessage);
    //    }
    //}
}

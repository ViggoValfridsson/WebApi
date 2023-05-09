using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Net;
using System.Text.RegularExpressions;
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
    public async Task<IActionResult> GetByEmail(string email)
    {
        try
        {
            if (Regex.IsMatch(email, @"^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$"))
                return BadRequest("Not a valid email address.");

            var user = await _userService.GetAsync(x => x.Email == email);

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



    // ändra bara pathen
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

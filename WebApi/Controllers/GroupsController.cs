using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using WebApi.Helpers.Repositories;
using WebApi.Helpers.Services;
using WebApi.Models.Exceptions;
using WebApi.Models.Schemas;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GroupsController : ControllerBase
{
    private readonly GroupService _groupService;
    private readonly GroupRepo _groupRepo;

    public GroupsController(GroupService groupService, GroupRepo groupRepo)
    {
        _groupService = groupService;
        _groupRepo = groupRepo;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var group = await _groupService.GetAsync(x => x.Id == id);

            if (group == null)
                return NotFound("Could not find a group with that id");

            return Ok(group);
        }
        catch (ApiException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.ErrorMessage);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetByName(string groupName)
    {
        try
        {
            var group = await _groupService.GetAsync(x => x.GroupName.ToLower() == groupName.ToLower());

            if (group == null)
                return NotFound("No group by that name exists.");

            return Ok(group);
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
            var groups = await _groupService.GetAllASync();

            if (!(groups.Any()) || groups == null)
                return NotFound("No groups could be found in the database.");

            return Ok(groups);
        }
        catch (ApiException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.ErrorMessage);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(GroupOrRoleCreateSchema schema)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var group = await _groupService.CreateAsync(schema);

                return Created("", group);
            }

            return BadRequest("Not a valid group name.");
        }
        catch (ApiException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.ErrorMessage);
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(GroupOrRoleUpdateSchema schema)
    {
        try
        {
            if (ModelState.IsValid)
            {
                if (!(await _groupRepo.AnyAsync(schema.Id)))
                    return NotFound("No group with the specified id could be found.");

                await _groupService.UpdateAsync(schema);

                var group = await _groupService.GetAsync(x => x.Id == schema.Id);

                return Ok(group);
            }

            return BadRequest("Not a valid schema.");
        }
        catch (ApiException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.ErrorMessage);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            if (!(await _groupRepo.AnyAsync(id)))
                return NotFound("No group with the specified id could be found.");

            await _groupService.DeleteAsync(x => x.Id == id);

            return NoContent();
        }
        catch (ApiException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.ErrorMessage);
        }
    }
}

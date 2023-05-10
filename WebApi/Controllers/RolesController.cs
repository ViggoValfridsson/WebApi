using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using WebApi.Helpers.Repositories;
using WebApi.Helpers.Services;
using WebApi.Models.Exceptions;
using WebApi.Models.Schemas;

namespace WebApi.Controllers;

[Route("api/roles")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly RoleService _roleService;
    private readonly RoleRepo _roleRepo;

    public RolesController(RoleService roleService, RoleRepo roleRepo)
    {
        _roleService = roleService;
        _roleRepo = roleRepo;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var role = await _roleService.GetAsync(x => x.Id == id);

            if (role == null)
                return NotFound("Could not find a role with that id");

            return Ok(role);
        }
        catch (ApiException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.ErrorMessage);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetByName(string roleName)
    {
        try
        {
            var role = await _roleService.GetAsync(x => x.RoleName.ToLower() == roleName.ToLower());

            if (role == null)
                return NotFound("No role by that name exists.");

            return Ok(role);
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
            var roles  = await _roleService.GetAllASync();

            if (roles.Count() < 1 || roles == null)
                return NotFound("No roles could be found in the database.");

            return Ok(roles);
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
                var role = await _roleService.CreateAsync(schema);

                return Created("", role);
            }

            return BadRequest("Not a valid role name.");
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
            if(ModelState.IsValid)
            {
                if (!(await _roleRepo.AnyAsync(schema.Id)))
                    return NotFound("No role with the specified id could be found.");

                var role = await _roleService.UpdateAsync(schema);

                return Ok(role);
            }

            return BadRequest("Not a valid role schema.");
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
            if (!(await _roleRepo.AnyAsync(id)))
                return NotFound("No role with the specified id could be found.");

            var user = await _roleRepo.GetAsync(x => x.Id == id);

            await _roleRepo.DeleteAsync(user!);

            return NoContent();
        }
        catch (ApiException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.ErrorMessage);
        }
    }
}

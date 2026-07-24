using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.DTOs;
using TaskManagementApi.Services.Interfaces;

namespace TaskManagementApi.Controllers;

[Authorize] // Requires authentication for all endpoints in this controller
[ApiController]
[Route("api/[controller]")]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _service;

    public ProjectController(IProjectService service)
    {
        _service = service;
    }

    // Accessible by any authenticated user (Admin or User)
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    // Accessible by any authenticated user (Admin or User)
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var project = await _service.GetByIdAsync(id);

        if (project == null)
            return NotFound("Project not found.");

        return Ok(project);
    }

    // Restricted to Admins only
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateProjectDto dto)
    {
        var project = await _service.CreateAsync(dto);

        return CreatedAtAction(
            nameof(Get),
            new { id = project.Id },
            project);
    }

    // Restricted to Admins only
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProjectDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);

        if (!result)
            return NotFound("Project not found.");

        return Ok("Project updated successfully.");
    }

    // Restricted to Admins only
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);

        if (!result)
            return NotFound("Project not found.");

        return Ok("Project deleted successfully.");
    }
}
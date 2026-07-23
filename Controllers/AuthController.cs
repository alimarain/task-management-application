using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagementApi.DTOs.Auth;
using TaskManagementApi.Models.Responses;
using TaskManagementApi.Services.Interfaces;

namespace TaskManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service)
    {
        _service = service;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var result = await _service.RegisterAsync(dto);

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var result = await _service.LoginAsync(dto);

        return Ok(result);
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        return Ok(new
        {
            Id = User.FindFirstValue(ClaimTypes.NameIdentifier),
            Name = User.FindFirstValue(ClaimTypes.Name),
            Email = User.FindFirstValue(ClaimTypes.Email),
            Role = User.FindFirstValue(ClaimTypes.Role)
        });
    }

    [HttpPost("refresh")]
public async Task<IActionResult> RefreshToken(
    RefreshTokenDto dto)
{
    var result = await _service.RefreshTokenAsync(dto);

    if (result == null)
        return Unauthorized();

    return Ok(result);
}
[HttpPost("logout")]
public async Task<IActionResult> Logout(
    RefreshTokenDto dto)
{
    await _service.LogoutAsync(dto.RefreshToken);

    return Ok(new ApiResponse<string>(
        true,
        "Logged out successfully.",
        null));
}
}
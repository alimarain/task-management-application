using Microsoft.AspNetCore.Identity;
using TaskManagementApi.DTOs.Auth;
using TaskManagementApi.Models.Entities;
using TaskManagementApi.Repositories.Interfaces;
using TaskManagementApi.Services.Interfaces;

namespace TaskManagementApi.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _repository;
    private readonly JwtService _jwtService;

    private readonly PasswordHasher<User> _passwordHasher = new();

    public AuthService(
        IAuthRepository repository,
        JwtService jwtService)
    {
        _repository = repository;
        _jwtService = jwtService;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        var existing =
            await _repository.GetByEmailAsync(dto.Email);

        if (existing != null)
            throw new Exception("Email already exists.");

        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email
        };

        user.PasswordHash =
            _passwordHasher.HashPassword(user, dto.Password);

        await _repository.AddUserAsync(user);

        return new AuthResponseDto
        {
            Token = _jwtService.GenerateToken(user),
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var user =
            await _repository.GetByEmailAsync(dto.Email);

        if (user == null)
            throw new Exception("Invalid credentials.");

        var result =
            _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                dto.Password);

        if (result == PasswordVerificationResult.Failed)
            throw new Exception("Invalid credentials.");

        return new AuthResponseDto
        {
            Token = _jwtService.GenerateToken(user),
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role
        };
    }
}
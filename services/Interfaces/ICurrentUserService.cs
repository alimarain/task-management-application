using System.Security.Claims;

namespace TaskManagementApi.Services.Interfaces;

public interface ICurrentUserService
{
    int UserId { get; }

    string Email { get; }

    string Role { get; }

    string FullName { get; }

    ClaimsPrincipal User { get; }
}
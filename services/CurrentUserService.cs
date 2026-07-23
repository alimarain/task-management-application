using System.Security.Claims;
using TaskManagementApi.Services.Interfaces;

namespace TaskManagementApi.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _contextAccessor;

    public CurrentUserService(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public ClaimsPrincipal User =>
        _contextAccessor.HttpContext?.User!;

    public int UserId =>
        int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    public string Email =>
        User.FindFirst(ClaimTypes.Email)!.Value;

    public string Role =>
        User.FindFirst(ClaimTypes.Role)!.Value;

    public string FullName =>
        User.FindFirst(ClaimTypes.Name)!.Value;
}
using TaskManagementApi.Models.Entities;

namespace TaskManagementApi.Repositories.Interfaces;

public interface IAuthRepository
{
    Task<User?> GetByEmailAsync(string email);

    Task AddUserAsync(User user);
    Task UpdateAsync(User user);

    Task<User?> GetByRefreshTokenAsync(string refreshToken);

}
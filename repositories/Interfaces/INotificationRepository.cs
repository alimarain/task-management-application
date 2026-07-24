using TaskManagementApi.Models.Entities;

namespace TaskManagementApi.Repositories.Interfaces;

public interface INotificationRepository
{
    Task<List<Notification>> GetByUserIdAsync(int userId);
    Task<Notification?> GetByIdAsync(int id);
    Task AddAsync(Notification notification);
    Task<Notification?> GetByIdAndUserIdAsync(int id, int userId);
    Task DeleteAsync(Notification notification);
    Task<List<Notification>> GetUnreadAsync(int userId);
    Task SaveChangesAsync();

}
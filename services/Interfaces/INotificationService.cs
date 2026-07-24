using TaskManagementApi.DTOs;

namespace TaskManagementApi.Services.Interfaces;

public interface INotificationService
{
    Task<List<NotificationDto>> GetMyNotificationsAsync();

    Task<int> GetUnreadCountAsync();

    Task MarkAsReadAsync(int id);

    Task CreateAsync(
        int userId,
        string title,
        string message);
    Task DeleteAsync(int id);
    Task MarkAllAsReadAsync();
}
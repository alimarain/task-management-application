using TaskManagementApi.DTOs;
using TaskManagementApi.Models.Entities;
using TaskManagementApi.Repositories.Interfaces;
using TaskManagementApi.Services.Interfaces;

namespace TaskManagementApi.Services.Implementations;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _repository;
    private readonly ICurrentUserService _currentUser;

    public NotificationService(
        INotificationRepository repository,
        ICurrentUserService currentUser)
    {
        _repository = repository;
        _currentUser = currentUser;
    }

    public async Task<List<NotificationDto>> GetMyNotificationsAsync()
    {
        var notifications =
            await _repository.GetByUserIdAsync(_currentUser.UserId);

        return notifications.Select(x => new NotificationDto
        {
            Id = x.Id,
            Title = x.Title,
            Message = x.Message,
            IsRead = x.IsRead,
            CreatedAt = x.CreatedAt
        }).ToList();
    }

    public async Task<int> GetUnreadCountAsync()
    {
        var notifications =
            await _repository.GetByUserIdAsync(_currentUser.UserId);

        return notifications.Count(x => !x.IsRead);
    }

    public async Task MarkAsReadAsync(int id)
    {
        var notification =
    await _repository.GetByIdAndUserIdAsync(
        id,
        _currentUser.UserId);

        if (notification == null)
            throw new Exception("Notification not found.");

        notification.IsRead = true;

        await _repository.SaveChangesAsync();
    }

    public async Task CreateAsync(
        int userId,
        string title,
        string message)
    {
        var notification = new Notification
        {
            UserId = userId,
            Title = title,
            Message = message
        };

        await _repository.AddAsync(notification);

        await _repository.SaveChangesAsync();
    }
public async Task DeleteAsync(int id)
{
    var notification =
        await _repository.GetByIdAndUserIdAsync(
            id,
            _currentUser.UserId);

    if (notification == null)
        throw new Exception("Notification not found.");

    await _repository.DeleteAsync(notification);

    await _repository.SaveChangesAsync();
}
public async Task MarkAllAsReadAsync()
{
    var notifications =
        await _repository.GetUnreadAsync(
            _currentUser.UserId);

    foreach (var notification in notifications)
    {
        notification.IsRead = true;
    }

    await _repository.SaveChangesAsync();
}
}
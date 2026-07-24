using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Data;
using TaskManagementApi.Models.Entities;
using TaskManagementApi.Repositories.Interfaces;

namespace TaskManagementApi.Repositories.Implementations;

public class NotificationRepository : INotificationRepository
{
    private readonly AppDbContext _context;

    public NotificationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Notification>> GetByUserIdAsync(int userId)
    {
        return await _context.Notifications
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<Notification?> GetByIdAsync(int id)
    {
        return await _context.Notifications
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(Notification notification)
    {
        await _context.Notifications.AddAsync(notification);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
    public async Task<Notification?> GetByIdAndUserIdAsync(
    int id,
    int userId)
{
    return await _context.Notifications
        .FirstOrDefaultAsync(x =>
            x.Id == id &&
            x.UserId == userId);
}
public Task DeleteAsync(Notification notification)
{
    _context.Notifications.Remove(notification);

    return Task.CompletedTask;
}
public async Task<List<Notification>> GetUnreadAsync(int userId)
{
    return await _context.Notifications
        .Where(x =>
            x.UserId == userId &&
            !x.IsRead)
        .OrderByDescending(x => x.CreatedAt)
        .ToListAsync();
}
}
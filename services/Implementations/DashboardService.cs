using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Constants;
using TaskManagementApi.Data;
using TaskManagementApi.DTOs.Dashboard;
using TaskManagementApi.Services.Interfaces;

namespace TaskManagementApi.Services.Implementations;

public class DashboardService : IDashboardService
{
    private readonly AppDbContext _context;

    public DashboardService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardDto> GetDashboardAsync()
    {
        return new DashboardDto
        {
            TotalProjects = await _context.Projects.CountAsync(),

            TotalTasks = await _context.Tasks.CountAsync(),

            CompletedTasks =
                await _context.Tasks.CountAsync(x =>
                    x.Status == TaskItemStatus.Completed),

            PendingTasks =
                await _context.Tasks.CountAsync(x =>
                    x.Status == TaskItemStatus.Pending),

            InProgressTasks =
                await _context.Tasks.CountAsync(x =>
                    x.Status == TaskItemStatus.InProgress),

            HighPriorityTasks =
                await _context.Tasks.CountAsync(x =>
                    x.Priority == TaskPriority.High),

            TotalUsers =
                await _context.Users.CountAsync()
        };
    }
}
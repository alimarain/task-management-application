using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Data;
using TaskManagementApi.Models.Entities;
using TaskManagementApi.Models.QueryParameters;
using TaskManagementApi.Repositories.Interfaces;

namespace TaskManagementApi.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;

    public TaskRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<TaskItem>> GetAllAsync(TaskQueryParameters query)
    {
        var tasks = _context.Tasks
            .Where(t => !t.IsDeleted)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            tasks = tasks.Where(x => x.Title.Contains(query.Search));
        }

        if (query.ProjectId.HasValue)
        {
            tasks = tasks.Where(x => x.ProjectId == query.ProjectId);
        }

        if (query.AssignedUserId.HasValue)
        {
            tasks = tasks.Where(x => x.AssignedToUserId == query.AssignedUserId);
        }

        if (query.Status.HasValue)
        {
            tasks = tasks.Where(x => (int)x.Status == query.Status);
        }

        if (query.Priority.HasValue)
        {
            tasks = tasks.Where(x => (int)x.Priority == query.Priority);
        }

        return await tasks
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();
    }

    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        return await _context.Tasks
            .Include(x => x.Project)
            .Include(x => x.AssignedToUser)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task<TaskItem> CreateAsync(TaskItem task)
    {
        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();
        return task;
    }

    public async Task<bool> UpdateAsync(int id, TaskItem task)
    {
        var existing = await _context.Tasks
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        if (existing == null)
            return false;

        existing.Title = task.Title;
        existing.Description = task.Description;
        existing.Priority = task.Priority;
        existing.Status = task.Status;
        existing.DueDate = task.DueDate;
        existing.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        if (task == null)
            return false;

        task.IsDeleted = true;
        task.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }
}
using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Data;
using TaskManagementApi.Extensions;
using TaskManagementApi.Models.Entities;
using TaskManagementApi.Models.QueryParameters;
using TaskManagementApi.Models.Responses;
using TaskManagementApi.Repositories.Interfaces;

namespace TaskManagementApi.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;

    public TaskRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<TaskItem>> GetAllAsync(
        TaskQueryParameters parameters)
    {
        var query = _context.Tasks
            .Include(t => t.Project)
            .Include(t => t.AssignedToUser)
            .AsQueryable();

        // Search
        if (!string.IsNullOrWhiteSpace(parameters.Search))
        {
            query = query.Where(t =>
                t.Title.Contains(parameters.Search) ||
                t.Description.Contains(parameters.Search));
        }

        // Filters
        if (parameters.ProjectId.HasValue)
            query = query.Where(t =>
                t.ProjectId == parameters.ProjectId);

        if (parameters.AssignedUserId.HasValue)
            query = query.Where(t =>
                t.AssignedToUserId == parameters.AssignedUserId);

        if (parameters.Priority.HasValue)
            query = query.Where(t =>
                (int)t.Priority == parameters.Priority);

        if (parameters.Status.HasValue)
            query = query.Where(t =>
                (int)t.Status == parameters.Status);

        // Sorting
        query = parameters.SortBy?.ToLower() switch
        {
            "title" => parameters.Descending
                ? query.OrderByDescending(t => t.Title)
                : query.OrderBy(t => t.Title),

            "duedate" => parameters.Descending
                ? query.OrderByDescending(t => t.DueDate)
                : query.OrderBy(t => t.DueDate),

            _ => query.OrderByDescending(t => t.Id)
        };

        return await query.ToPagedResultAsync(parameters);
    }

    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        return await _context.Tasks
            .Include(t => t.Project)
            .Include(t => t.AssignedToUser)
            .FirstOrDefaultAsync(x => x.Id == id);
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
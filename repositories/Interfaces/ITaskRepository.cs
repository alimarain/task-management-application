using TaskManagementApi.Models.Entities;
using TaskManagementApi.Models.QueryParameters;

namespace TaskManagementApi.Repositories.Interfaces;

public interface ITaskRepository
{
    Task<List<TaskItem>> GetAllAsync(TaskQueryParameters query);

    Task<TaskItem?> GetByIdAsync(int id);

    Task<TaskItem> CreateAsync(TaskItem task);

    Task<bool> UpdateAsync(int id, TaskItem task);

    Task<bool> DeleteAsync(int id);
}
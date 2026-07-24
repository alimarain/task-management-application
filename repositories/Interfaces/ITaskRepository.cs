using TaskManagementApi.Models.Entities;
using TaskManagementApi.Models.QueryParameters;
using TaskManagementApi.Models.Responses;

namespace TaskManagementApi.Repositories.Interfaces;

public interface ITaskRepository
{
    Task<PagedResult<TaskItem>> GetAllAsync(TaskQueryParameters parameters);
    Task<TaskItem?> GetByIdAsync(int id);

    Task<TaskItem> CreateAsync(TaskItem task);

    Task<bool> UpdateAsync(int id, TaskItem task);

    Task<bool> DeleteAsync(int id);
}
using TaskManagementApi.Models.DTOs;
using TaskManagementApi.Models.QueryParameters;

namespace TaskManagementApi.Services.Interfaces;

public interface ITaskService
{
    Task<List<TaskDto>> GetAllAsync(TaskQueryParameters query);

    Task<TaskDto?> GetByIdAsync(int id);

    Task<TaskDto> CreateAsync(CreateTaskDto dto);

    Task<bool> UpdateAsync(int id, UpdateTaskDto dto);

    Task<bool> DeleteAsync(int id);
}
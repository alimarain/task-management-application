using TaskManagementApi.DTOs;
using TaskManagementApi.Models.QueryParameters;
using TaskManagementApi.Models.Responses;

namespace TaskManagementApi.Services.Interfaces;

public interface ITaskService
{
    Task<PagedResult<TaskDto>> GetAllAsync(
    TaskQueryParameters parameters);

    Task<TaskDto?> GetByIdAsync(int id);

    Task<TaskDto> CreateAsync(CreateTaskDto dto);

    Task<bool> UpdateAsync(int id, UpdateTaskDto dto);

    Task<bool> DeleteAsync(int id);
}
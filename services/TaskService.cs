using TaskManagementApi.DTOs;
using TaskManagementApi.Models.Entities;
using TaskManagementApi.Models.QueryParameters;
using TaskManagementApi.Repositories.Interfaces;
using TaskManagementApi.Services.Interfaces;

namespace TaskManagementApi.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _repository;

    public TaskService(ITaskRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<TaskDto>> GetAllAsync(TaskQueryParameters query)
    {
        var tasks = await _repository.GetAllAsync(query);

        return tasks.Select(task => new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status.ToString(),
            Priority = task.Priority.ToString(),
            DueDate = task.DueDate,
            ProjectId = task.ProjectId,
            AssignedToUserId = task.AssignedToUserId
        }).ToList();
    }

    public async Task<TaskDto?> GetByIdAsync(int id)
    {
        var task = await _repository.GetByIdAsync(id);

        if (task == null)
            return null;

        return new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status.ToString(),
            Priority = task.Priority.ToString(),
            DueDate = task.DueDate,
            ProjectName = task.Project?.Name ?? string.Empty,
            AssignedUser = task.AssignedToUser?.FullName ?? string.Empty
        };
    }

    public async Task<TaskDto> CreateAsync(CreateTaskDto dto)
    {
        var task = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            ProjectId = dto.ProjectId,
            AssignedToUserId = dto.AssignedToUserId,
            Priority = (Constants.TaskPriority)dto.Priority,
            Status = Constants.TaskItemStatus.Pending,
            DueDate = dto.DueDate
        };

        await _repository.CreateAsync(task);

        var created = await _repository.GetByIdAsync(task.Id);

        return new TaskDto
        {
            Id = created!.Id,
            Title = created.Title,
            Description = created.Description,
            Status = created.Status.ToString(),
            Priority = created.Priority.ToString(),
            DueDate = created.DueDate,
            ProjectName = created.Project?.Name ?? string.Empty,
            AssignedUser = created.AssignedToUser?.FullName ?? string.Empty
        };
    }

    public async Task<bool> UpdateAsync(int id, UpdateTaskDto dto)
    {
        var task = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            Priority = (Constants.TaskPriority)dto.Priority,
            Status = (Constants.TaskItemStatus)dto.Status,
            DueDate = dto.DueDate
        };

        return await _repository.UpdateAsync(id, task);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}
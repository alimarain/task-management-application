using AutoMapper;
using TaskManagementApi.DTOs;
using TaskManagementApi.Models.Entities;
using TaskManagementApi.Models.QueryParameters;
using TaskManagementApi.Models.Responses;
using TaskManagementApi.Repositories.Interfaces;
using TaskManagementApi.Services.Interfaces;

namespace TaskManagementApi.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _repository;
    private readonly INotificationService _notificationService;
    private readonly IMapper _mapper;

    public TaskService(
        ITaskRepository repository,
        INotificationService notificationService,
        IMapper mapper)
    {
        _repository = repository;
        _notificationService = notificationService;
        _mapper = mapper;
    }

    public async Task<PagedResult<TaskDto>> GetAllAsync(TaskQueryParameters parameters)
    {
        var result = await _repository.GetAllAsync(parameters);

        return new PagedResult<TaskDto>
        {
            Items = _mapper.Map<IEnumerable<TaskDto>>(result.Items),
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }

    public async Task<TaskDto?> GetByIdAsync(int id)
    {
        var task = await _repository.GetByIdAsync(id);

        if (task == null)
            return null;

        return _mapper.Map<TaskDto>(task);
    }

    public async Task<TaskDto> CreateAsync(CreateTaskDto dto)
    {
        var task = _mapper.Map<TaskItem>(dto);
        task.Status = Constants.TaskItemStatus.Pending;

        await _repository.CreateAsync(task);

        // Automatically trigger notification for task assignment
        await _notificationService.CreateAsync(
            task.AssignedToUserId,
            "New Task Assigned",
            $"Task '{task.Title}' has been assigned to you.");

        var created = await _repository.GetByIdAsync(task.Id);

        return _mapper.Map<TaskDto>(created);
    }

    public async Task<bool> UpdateAsync(int id, UpdateTaskDto dto)
    {
        var existingTask = await _repository.GetByIdAsync(id);
        if (existingTask == null)
            return false;

        var newStatus = (Constants.TaskItemStatus)dto.Status;

        var taskToUpdate = _mapper.Map<TaskItem>(dto);
        var updated = await _repository.UpdateAsync(id, taskToUpdate);

        if (updated)
        {
            // Trigger notification if status changed to Completed
            if (existingTask.Status != Constants.TaskItemStatus.Completed && 
                newStatus == Constants.TaskItemStatus.Completed)
            {
                await _notificationService.CreateAsync(
                    existingTask.AssignedToUserId,
                    "Task Completed",
                    $"You completed '{existingTask.Title}'.");
            }
        }

        return updated;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}
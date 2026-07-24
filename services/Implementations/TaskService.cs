using AutoMapper;
using TaskManagementApi.DTOs;
using TaskManagementApi.Models.Entities;
using TaskManagementApi.Models.QueryParameters;
using TaskManagementApi.Repositories.Interfaces;
using TaskManagementApi.Services.Interfaces;

namespace TaskManagementApi.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _repository;
    private readonly IMapper _mapper;

    public TaskService(ITaskRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<TaskDto>> GetAllAsync(TaskQueryParameters query)
    {
        var tasks = await _repository.GetAllAsync(query);

        return _mapper.Map<List<TaskDto>>(tasks);
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

        var created = await _repository.GetByIdAsync(task.Id);

        return _mapper.Map<TaskDto>(created);
    }

    public async Task<bool> UpdateAsync(int id, UpdateTaskDto dto)
    {
        var task = _mapper.Map<TaskItem>(dto);

        return await _repository.UpdateAsync(id, task);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}
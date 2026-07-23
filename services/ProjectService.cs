using TaskManagementApi.Models.DTOs;
using TaskManagementApi.Models.Entities;
using TaskManagementApi.Repositories.Interfaces;
using TaskManagementApi.Services.Interfaces;

namespace TaskManagementApi.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _repository;

    public ProjectService(IProjectRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ProjectDto>> GetAllAsync()
    {
        var projects = await _repository.GetAllAsync();

        return projects.Select(x => new ProjectDto
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            OwnerId = x.OwnerId,
            OwnerName = x.Owner.FullName,
            TotalTasks = x.Tasks.Count
        }).ToList();
    }

    public async Task<ProjectDto?> GetByIdAsync(int id)
    {
        var project = await _repository.GetByIdAsync(id);

        if (project == null)
            return null;

        return new ProjectDto
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            OwnerId = project.OwnerId,
            OwnerName = project.Owner.FullName,
            TotalTasks = project.Tasks.Count
        };
    }

    public async Task<ProjectDto> CreateAsync(CreateProjectDto dto)
    {
        var project = new Project
        {
            Name = dto.Name,
            Description = dto.Description,
            OwnerId = dto.OwnerId
        };

        await _repository.AddAsync(project);

        var created = await _repository.GetByIdAsync(project.Id);

        return new ProjectDto
        {
            Id = created!.Id,
            Name = created.Name,
            Description = created.Description,
            OwnerId = created.OwnerId,
            OwnerName = created.Owner.FullName,
            TotalTasks = created.Tasks.Count
        };
    }

    public async Task<bool> UpdateAsync(int id, UpdateProjectDto dto)
    {
        var project = new Project
        {
            Name = dto.Name,
            Description = dto.Description
        };

        return await _repository.UpdateAsync(id, project);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}
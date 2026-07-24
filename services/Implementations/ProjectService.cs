using AutoMapper;
using TaskManagementApi.DTOs;
using TaskManagementApi.Models.Entities;
using TaskManagementApi.Repositories.Interfaces;
using TaskManagementApi.Services.Interfaces;

namespace TaskManagementApi.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _repository;
    private readonly IMapper _mapper;

    public ProjectService(IProjectRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<ProjectDto>> GetAllAsync()
    {
        var projects = await _repository.GetAllAsync();

        return _mapper.Map<List<ProjectDto>>(projects);
    }

    public async Task<ProjectDto?> GetByIdAsync(int id)
    {
        var project = await _repository.GetByIdAsync(id);

        if (project == null)
            return null;

        return _mapper.Map<ProjectDto>(project);
    }

    public async Task<ProjectDto> CreateAsync(CreateProjectDto dto)
    {
        var project = _mapper.Map<Project>(dto);

        await _repository.AddAsync(project);

        var created = await _repository.GetByIdAsync(project.Id);

        return _mapper.Map<ProjectDto>(created);
    }

    public async Task<bool> UpdateAsync(int id, UpdateProjectDto dto)
    {
        var project = _mapper.Map<Project>(dto);

        return await _repository.UpdateAsync(id, project);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}
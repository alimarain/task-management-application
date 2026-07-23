using TaskManagementApi.Models.Entities;

namespace TaskManagementApi.Repositories.Interfaces;

public interface IProjectRepository
{
    Task<List<Project>> GetAllAsync();

    Task<Project?> GetByIdAsync(int id);

    Task AddAsync(Project project);

    Task<bool> UpdateAsync(int id, Project project);

    Task<bool> DeleteAsync(int id);
}
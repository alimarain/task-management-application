using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Data;
using TaskManagementApi.Models.Entities;
using TaskManagementApi.Repositories.Interfaces;

namespace TaskManagementApi.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly AppDbContext _context;

    public ProjectRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Project>> GetAllAsync()
    {
        return await _context.Projects
            .Include(x => x.Owner)
            .Include(x => x.Tasks)
            .Where(x => !x.IsDeleted)
            .ToListAsync();
    }

    public async Task<Project?> GetByIdAsync(int id)
    {
        return await _context.Projects
            .Include(x => x.Owner)
            .Include(x => x.Tasks)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task AddAsync(Project project)
    {
        await _context.Projects.AddAsync(project);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> UpdateAsync(int id, Project project)
    {
        var existing = await _context.Projects
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        if (existing == null)
            return false;

        existing.Name = project.Name;
        existing.Description = project.Description;
        existing.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var project = await _context.Projects
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        if (project == null)
            return false;

        project.IsDeleted = true;
        project.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }
}
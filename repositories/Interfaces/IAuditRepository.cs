using TaskManagementApi.Models.Entities;

namespace TaskManagementApi.Repositories.Interfaces;

public interface IAuditRepository
{
    Task AddAsync(AuditLog log);

    Task<List<AuditLog>> GetAllAsync();
}
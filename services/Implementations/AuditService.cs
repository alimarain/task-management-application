using TaskManagementApi.DTOs.Audit;
using TaskManagementApi.Models.Entities;
using TaskManagementApi.Repositories.Interfaces;
using TaskManagementApi.Services.Interfaces;

namespace TaskManagementApi.Services.Implementations;

public class AuditService : IAuditService
{
    private readonly IAuditRepository _repository;

    public AuditService(IAuditRepository repository)
    {
        _repository = repository;
    }

    public async Task LogAsync(
        int userId,
        string userName,
        string action,
        string entity,
        int entityId,
        string description)
    {
        var log = new AuditLog
        {
            UserId = userId,
            UserName = userName,
            Action = action,
            EntityName = entity,
            EntityId = entityId,
            Description = description
        };

        await _repository.AddAsync(log);
    }

    public async Task<List<AuditLogDto>> GetAllAsync()
    {
        var logs = await _repository.GetAllAsync();

        return logs.Select(log => new AuditLogDto
        {
            Id = log.Id,
            UserName = log.UserName,
            Action = log.Action,
            EntityName = log.EntityName,
            EntityId = log.EntityId,
            Description = log.Description,
            CreatedAt = log.CreatedAt
        }).ToList();
    }
}
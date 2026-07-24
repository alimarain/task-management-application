using TaskManagementApi.DTOs.Audit;

namespace TaskManagementApi.Services.Interfaces;

public interface IAuditService
{
    Task LogAsync(
        int userId,
        string userName,
        string action,
        string entity,
        int entityId,
        string description);

    Task<List<AuditLogDto>> GetAllAsync();
}
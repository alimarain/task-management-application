namespace TaskManagementApi.Models.Entities;

public class AuditLog : BaseEntity
{
    public int UserId { get; set; }

    public string UserName { get; set; } = string.Empty;

    public string Action { get; set; } = string.Empty;

    public string EntityName { get; set; } = string.Empty;

    public int EntityId { get; set; }

    public string Description { get; set; } = string.Empty;
}
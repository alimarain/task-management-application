namespace TaskManagementApi.Models.DTOs;

public class ProjectDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int OwnerId { get; set; }

    public string OwnerName { get; set; } = string.Empty;

    public int TotalTasks { get; set; }
}
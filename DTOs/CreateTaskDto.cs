namespace TaskManagementApi.Models.DTOs;

public class CreateTaskDto
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int ProjectId { get; set; }

    public int AssignedToUserId { get; set; }

    public int Priority { get; set; }

    public DateTime DueDate { get; set; }
}
namespace TaskManagementApi.Models.DTOs;

public class UpdateTaskDto
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int Priority { get; set; }

    public int Status { get; set; }

    public DateTime DueDate { get; set; }
}
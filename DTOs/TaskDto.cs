namespace TaskManagementApi.Models.DTOs;

public class TaskDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public string Priority { get; set; } = string.Empty;

    public DateTime DueDate { get; set; }

    public string ProjectName { get; set; } = string.Empty;

    public string AssignedUser { get; set; } = string.Empty;
        public int ProjectId { get; set; }


        public int AssignedToUserId { get; set; }

}
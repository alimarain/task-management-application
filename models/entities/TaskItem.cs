using TaskManagementApi.Constants;

namespace TaskManagementApi.Models.Entities;

public class TaskItem : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TaskItemStatus Status { get; set; } = TaskItemStatus.Pending;
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public DateTime DueDate { get; set; }
    public int ProjectId { get; set; }
    public Project Project { get; set; } = null!;
    public int AssignedToUserId { get; set; }
    public User AssignedToUser { get; set; } = null!;
    public ICollection<Attachment> Attachments { get; set; }
    = new List<Attachment>();
}
namespace TaskManagementApi.Models.Entities;

public class Project : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int OwnerId { get; set; }
    public User Owner { get; set; } = null!;
    public ICollection<TaskItem> Tasks { get; set; }
        = new List<TaskItem>();
    public ICollection<Attachment> Attachments { get; set; }
    = new List<Attachment>();
}
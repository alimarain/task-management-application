namespace TaskManagementApi.Models.Entities;

public class Attachment : BaseEntity
{
    public string FileName { get; set; } = string.Empty;

    public string StoredFileName { get; set; } = string.Empty;

    public string ContentType { get; set; } = string.Empty;

    public long FileSize { get; set; }

    public string FilePath { get; set; } = string.Empty;

    public int UploadedByUserId { get; set; }

    public User UploadedByUser { get; set; } = null!;

    public int? ProjectId { get; set; }

    public Project? Project { get; set; }

    public int? TaskId { get; set; }

    public TaskItem? Task { get; set; }
}
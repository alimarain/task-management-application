namespace TaskManagementApi.Models.Entities;

public class User : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "User";
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

    public ICollection<Project> Projects { get; set; }
        = new List<Project>();

    public ICollection<TaskItem> AssignedTasks { get; set; }
        = new List<TaskItem>();
}
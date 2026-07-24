namespace TaskManagementApi.DTOs.Dashboard;

public class DashboardDto
{
    public int TotalProjects { get; set; }

    public int TotalTasks { get; set; }

    public int CompletedTasks { get; set; }

    public int PendingTasks { get; set; }

    public int InProgressTasks { get; set; }

    public int HighPriorityTasks { get; set; }

    public int TotalUsers { get; set; }
}
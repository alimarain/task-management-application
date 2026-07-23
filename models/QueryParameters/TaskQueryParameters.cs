namespace TaskManagementApi.Models.QueryParameters;

public class TaskQueryParameters
{
    public string? Search { get; set; }

    public int? ProjectId { get; set; }

    public int? AssignedUserId { get; set; }

    public int? Status { get; set; }

    public int? Priority { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}
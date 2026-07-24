using TaskManagementApi.Models.QueryParameters;

namespace TaskManagementApi.Models.QueryParameters;

public class TaskQueryParameters : PaginationParameters
{
    public int? ProjectId { get; set; }

    public int? AssignedUserId { get; set; }

    public int? Status { get; set; }

    public int? Priority { get; set; }
}
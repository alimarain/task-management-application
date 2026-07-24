using TaskManagementApi.DTOs.Dashboard;

namespace TaskManagementApi.Services.Interfaces;

public interface IDashboardService
{
    Task<DashboardDto> GetDashboardAsync();
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.Services.Interfaces;

namespace TaskManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _service;

    public NotificationController(
        INotificationService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetMyNotifications()
    {
        var notifications =
            await _service.GetMyNotificationsAsync();

        return Ok(notifications);
    }

    [HttpGet("unread-count")]
    public async Task<IActionResult> GetUnreadCount()
    {
        var count =
            await _service.GetUnreadCountAsync();

        return Ok(count);
    }

    [HttpPut("{id}/read")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        await _service.MarkAsReadAsync(id);

        return Ok(new
        {
            Message = "Notification marked as read."
        });
    }

    [HttpDelete("{id}")]
public async Task<IActionResult> Delete(int id)
{
    await _service.DeleteAsync(id);

    return Ok(new
    {
        Message = "Notification deleted successfully."
    });
}
[HttpPut("read-all")]
public async Task<IActionResult> MarkAllAsRead()
{
    await _service.MarkAllAsReadAsync();

    return Ok(new
    {
        Message = "All notifications marked as read."
    });
}
}
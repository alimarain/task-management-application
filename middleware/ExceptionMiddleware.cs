using System.Text.Json;
using StudentApi.Responses;

namespace TaskManagementApi.Middleware;
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(
        HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                ex.Message);

            context.Response.StatusCode = 500;

            context.Response.ContentType = "application/json";

            var response =
                new ApiResponse<object>(
                    false,
                    "Something went wrong.",
                    null);

            var json =
                JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
    }
}
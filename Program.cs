using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using TaskManagementApi.Data;
using TaskManagementApi.Middleware;
using TaskManagementApi.Repositories;
using TaskManagementApi.Repositories.Interfaces;
using TaskManagementApi.Services;
using TaskManagementApi.Services.Interfaces;
using TaskManagementApi.Mapping;
using TaskManagementApi.Configuration;
using TaskManagementApi.Logging;
using Serilog;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using TaskManagementApi.Services.Implementations;
using TaskManagementApi.Repositories.Implementations;



SerilogLoggingExtensions.ConfigureSerilog();
var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

// Strongly Typed Settings
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt"));

// Read JwtSettings directly for authentication setup
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>() 
    ?? throw new InvalidOperationException("JwtSettings section is missing from configuration.");

// Controllers & Validation
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// API Versioning Configuration
builder.Services
.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1,0);

    options.AssumeDefaultVersionWhenUnspecified = true;

    options.ReportApiVersions = true;
});
// Rate Limiting Configuration
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("ApiLimiter", configure =>
    {
        configure.Window = TimeSpan.FromMinutes(1);

        configure.PermitLimit = 100;

        configure.QueueLimit = 0;
    });
});


// Response Compression Registration
builder.Services.AddResponseCompression();

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});
// Health Checks
builder.Services.AddHealthChecks().AddDbContextCheck<AppDbContext>();

// Dependency Injection
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IAuditRepository, AuditRepository>();
builder.Services.AddScoped<IAuditService, AuditService>();
builder.Services.AddScoped<JwtService>();

// JWT Authentication using JwtSettings
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings.Key)),
        RoleClaimType = System.Security.Claims.ClaimTypes.Role
    };
});

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    await DbSeeder.SeedAdminAsync(scope.ServiceProvider);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<CorrelationIdMiddleware>();

app.UseSerilogRequestLogging();
// Enable Response Compression Middleware early in the pipeline
app.UseResponseCompression();
//middleware for rate limiting
app.UseRateLimiter();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
// Map Health Check Endpoint
app.MapHealthChecks("/health");

// Map Controllers with Global Rate Limiting
app.MapControllers()
   .RequireRateLimiting("ApiLimiter");
app.Run();
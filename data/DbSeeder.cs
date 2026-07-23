using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Models.Entities;

namespace TaskManagementApi.Data;

public static class DbSeeder
{
    public static async Task SeedAdminAsync(
        IServiceProvider services)
    {
        var context =
            services.GetRequiredService<AppDbContext>();

        if (await context.Users.AnyAsync(x =>
            x.Email == "admin@task.com"))
            return;

        var hasher = new PasswordHasher<User>();

        var admin = new User
        {
            FullName = "System Admin",
            Email = "admin@task.com",
            Role = "Admin"
        };

        admin.PasswordHash =
            hasher.HashPassword(admin, "Admin123!");

        context.Users.Add(admin);

        await context.SaveChangesAsync();
    }
}
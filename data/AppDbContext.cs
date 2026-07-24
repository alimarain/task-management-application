using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Models.Entities;

namespace TaskManagementApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<TaskItem> Tasks => Set<TaskItem>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<Attachment> Attachments => Set<Attachment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //relationships
        modelBuilder.Entity<User>().HasQueryFilter(x => !x.IsDeleted);

        modelBuilder.Entity<Project>().HasQueryFilter(x => !x.IsDeleted);

        modelBuilder.Entity<TaskItem>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Notification>()
        .HasOne(x => x.User)
        .WithMany(x => x.Notifications)
        .HasForeignKey(x => x.UserId)
        .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasIndex(x => x.Email)
            .IsUnique();

        modelBuilder.Entity<Project>()
            .HasOne(x => x.Owner)
            .WithMany(x => x.Projects)
            .HasForeignKey(x => x.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TaskItem>()
            .HasOne(x => x.Project)
            .WithMany(x => x.Tasks)
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TaskItem>()
            .HasOne(x => x.AssignedToUser)
            .WithMany(x => x.AssignedTasks)
            .HasForeignKey(x => x.AssignedToUserId)
            .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Attachment>()
    .HasOne(x => x.Project)
    .WithMany(x => x.Attachments)
    .HasForeignKey(x => x.ProjectId)
    .OnDelete(DeleteBehavior.Cascade);

modelBuilder.Entity<Attachment>()
    .HasOne(x => x.Project)
    .WithMany(x => x.Attachments)
    .HasForeignKey(x => x.ProjectId)
    .OnDelete(DeleteBehavior.NoAction);

modelBuilder.Entity<Attachment>()
    .HasOne(x => x.Task)
    .WithMany(x => x.Attachments)
    .HasForeignKey(x => x.TaskId)
    .OnDelete(DeleteBehavior.NoAction);

    modelBuilder.Entity<Attachment>()
    .HasOne(x => x.UploadedByUser)
    .WithMany(x => x.Attachments)
    .HasForeignKey(x => x.UploadedByUserId)
    .OnDelete(DeleteBehavior.Restrict);
    }

}
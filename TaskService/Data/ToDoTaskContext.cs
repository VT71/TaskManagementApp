using Microsoft.EntityFrameworkCore;
using TaskService.Models;

namespace TaskService.Data;

public class ToDoTaskContext : DbContext
{
    public ToDoTaskContext(DbContextOptions<ToDoTaskContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ToDoTask>().HasData(
            new ToDoTask { Id = 1, Title = "Task 1", DueDate = DateTime.Parse("2025-10-27T15:23:59.689Z") },
            new ToDoTask { Id = 2, Title = "Task 2", DueDate = DateTime.Parse("2025-10-27T15:23:59.689Z"), Completed = true },
            new ToDoTask { Id = 3, Title = "Task 3", DueDate = DateTime.Parse("2025-10-27T15:23:59.689Z") });
    }
    public DbSet<ToDoTask> ToDoTasks { get; set; }
}
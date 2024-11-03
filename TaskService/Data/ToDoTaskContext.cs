using Microsoft.EntityFrameworkCore;
using TaskService.Models;

namespace TaskService.Data;

// Database context.
public class ToDoTaskContext : DbContext
{
    public ToDoTaskContext(DbContextOptions<ToDoTaskContext> options)
        : base(options)
    {
    }

    // Configures the model and seeds initial data for the ToDoTasks.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ToDoTask>().HasData(
            new ToDoTask { Id = 1, Title = "Task A", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean ut velit vel orci dapibus tristique.", DueDate = DateTime.Parse("2025-01-01T00:00:00.000Z"), Completed = false },
            new ToDoTask { Id = 2, Title = "Task B", Description = null, DueDate = DateTime.Parse("2025-01-02T00:00:00.000Z"), Completed = true },
            new ToDoTask { Id = 3, Title = "Task C", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque non orci sit amet turpis cursus.", DueDate = DateTime.Parse("2025-01-03T00:00:00.000Z"), Completed = false },
            new ToDoTask { Id = 4, Title = "Task D", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut a massa nec elit vestibulum aliquam.", DueDate = DateTime.Parse("2025-01-04T00:00:00.000Z"), Completed = true },
            new ToDoTask { Id = 5, Title = "Task E", Description = null, DueDate = DateTime.Parse("2025-01-05T00:00:00.000Z"), Completed = false },
            new ToDoTask { Id = 6, Title = "Task F", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Morbi volutpat tortor et magna bibendum.", DueDate = DateTime.Parse("2025-01-06T00:00:00.000Z"), Completed = true },
            new ToDoTask { Id = 7, Title = "Task G", Description = null, DueDate = DateTime.Parse("2025-01-07T00:00:00.000Z"), Completed = false },
            new ToDoTask { Id = 8, Title = "Task H", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse a risus ac sapien ultricies.", DueDate = DateTime.Parse("2025-01-08T00:00:00.000Z"), Completed = true },
            new ToDoTask { Id = 9, Title = "Task I", Description = null, DueDate = DateTime.Parse("2025-01-09T00:00:00.000Z"), Completed = false },
            new ToDoTask { Id = 10, Title = "Task J", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent vehicula nisl at elit feugiat.", DueDate = DateTime.Parse("2025-01-10T00:00:00.000Z"), Completed = true },
            new ToDoTask { Id = 11, Title = "Task K", Description = null, DueDate = DateTime.Parse("2025-01-11T00:00:00.000Z"), Completed = false },
            new ToDoTask { Id = 12, Title = "Task L", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur auctor orci sit amet mi.", DueDate = DateTime.Parse("2025-01-12T00:00:00.000Z"), Completed = true },
            new ToDoTask { Id = 13, Title = "Task M", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec tempor ligula eget cursus.", DueDate = DateTime.Parse("2025-01-13T00:00:00.000Z"), Completed = false },
            new ToDoTask { Id = 14, Title = "Task N", Description = null, DueDate = DateTime.Parse("2025-01-14T00:00:00.000Z"), Completed = true },
            new ToDoTask { Id = 15, Title = "Task O", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus scelerisque orci vitae metus.", DueDate = DateTime.Parse("2025-01-15T00:00:00.000Z"), Completed = false },
            new ToDoTask { Id = 16, Title = "Task P", Description = null, DueDate = DateTime.Parse("2025-01-16T00:00:00.000Z"), Completed = true },
            new ToDoTask { Id = 17, Title = "Task Q", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer at justo vel erat pharetra.", DueDate = DateTime.Parse("2025-01-17T00:00:00.000Z"), Completed = false },
            new ToDoTask { Id = 18, Title = "Task R", Description = null, DueDate = DateTime.Parse("2025-01-18T00:00:00.000Z"), Completed = true },
            new ToDoTask { Id = 19, Title = "Task S", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam et lacus pulvinar, porta eros.", DueDate = DateTime.Parse("2025-01-19T00:00:00.000Z"), Completed = false },
            new ToDoTask { Id = 20, Title = "Task T", Description = null, DueDate = DateTime.Parse("2025-01-20T00:00:00.000Z"), Completed = true },
            new ToDoTask { Id = 21, Title = "Task U", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum sit amet lacus lacinia.", DueDate = DateTime.Parse("2025-01-21T00:00:00.000Z"), Completed = false },
            new ToDoTask { Id = 22, Title = "Task V", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam non justo at orci commodo.", DueDate = DateTime.Parse("2025-01-22T00:00:00.000Z"), Completed = true },
            new ToDoTask { Id = 23, Title = "Task W", Description = null, DueDate = DateTime.Parse("2025-01-23T00:00:00.000Z"), Completed = false },
            new ToDoTask { Id = 24, Title = "Task X", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus consectetur orci id odio.", DueDate = DateTime.Parse("2025-01-24T00:00:00.000Z"), Completed = true },
            new ToDoTask { Id = 25, Title = "Task Y", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin sed felis vestibulum, lacinia.", DueDate = DateTime.Parse("2025-01-25T00:00:00.000Z"), Completed = false },
            new ToDoTask { Id = 26, Title = "Task Z", Description = null, DueDate = DateTime.Parse("2025-01-26T00:00:00.000Z"), Completed = true }
        );
    }
    public DbSet<ToDoTask> ToDoTasks { get; set; }
}
using Microsoft.EntityFrameworkCore;

namespace TaskService.Models;

public class ToDoTaskContext : DbContext
{
    public ToDoTaskContext(DbContextOptions<ToDoTaskContext> options)
        : base(options)
    {
    }

    public DbSet<ToDoTask> ToDoTasks { get; set; }
}
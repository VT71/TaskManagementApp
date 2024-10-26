using Microsoft.EntityFrameworkCore;
using TaskService.Models;

namespace TaskService.Data;

public class ToDoTaskContext : DbContext
{
    public ToDoTaskContext(DbContextOptions<ToDoTaskContext> options)
        : base(options)
    {
    }

    public DbSet<ToDoTask> ToDoTasks { get; set; }
}
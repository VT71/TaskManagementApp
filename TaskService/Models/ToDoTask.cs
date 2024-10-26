using System.ComponentModel.DataAnnotations;
using TaskService.Attributes;

namespace TaskService.Models;

public class ToDoTask
{
    public long Id { get; set; }
    [StringLength(100)]
    public required string Title { get; set; }
    public string? Description { get; set; }
    [FutureDate]
    public required DateTime DueDate { get; set; }
    public bool Completed { get; set; }
}
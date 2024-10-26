using System.ComponentModel.DataAnnotations;

namespace TaskService.Models;

public class Task
{
    public long Id { get; set; }
    [StringLength(100)]
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required DateTimeOffset DueDate { get; set; }
    public bool Completed { get; set; }
}
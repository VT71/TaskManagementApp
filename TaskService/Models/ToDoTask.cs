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

    public bool Equals(ToDoTask other)
    {
        if (other is null)
            return false;

        return Id == other.Id 
                && Title == other.Title 
                && Description == other.Description 
                && DueDate == other.DueDate 
                && Completed == other.Completed;
    }

    public override bool Equals(object obj) => Equals(obj as ToDoTask);
    public override int GetHashCode() => (Id, Title, Description, DueDate, Completed).GetHashCode();
}
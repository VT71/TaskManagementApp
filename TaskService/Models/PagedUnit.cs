namespace TaskService.Models;

public class PagedUnit<T>
{
    public int TotalCount { get; set; }
    public ICollection<T> Items { get; set; } = Array.Empty<T>();
}
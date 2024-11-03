namespace TaskService.Models;

// Represents a paginated response containing a total count and a collection of items.
public class PagedUnit<T>
{
    // Total number of items available across all pages.
    public int TotalCount { get; set; }

    // Items for the current page.
    public ICollection<T> Items { get; set; } = Array.Empty<T>();
}
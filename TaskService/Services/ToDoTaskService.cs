using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskService.Data;
using TaskService.Models;

namespace TaskService.Services;

public class ToDoTaskService
{
    private readonly ToDoTaskContext _context;
    public ToDoTaskService(ToDoTaskContext context)
    {
        _context = context;
    }

    // Retrieves all ToDoTasks from the database without tracking changes.
    public async Task<ICollection<ToDoTask>> GetAll()
    {
        return await _context.ToDoTasks.AsNoTracking().ToListAsync();
    }

    // Retrieves a specific ToDoTask by its ID, or null if not found.
    public async Task<ToDoTask?> GetById(long id)
    {
        return await _context.ToDoTasks.FindAsync(id);
    }

    // Retrieves ToDoTasks based on search criteria, including filtering, sorting, and pagination.
    public async Task<PagedUnit<ToDoTask>> GetByCriteria(string? titleSearch, string? sortBy, string? sortDirection, int page, int pageSize)
    {
        IQueryable<ToDoTask> query = _context.ToDoTasks;

        // Filter tasks if a title search is provided.
        if (!string.IsNullOrEmpty(titleSearch))
        {
            query = GetFiltered(titleSearch, query);
        }

        // Get the total count of tasks after filtering.
        int totalCount = await query.CountAsync();

        // Set default sort direction to ascending if not provided.
        if (string.IsNullOrEmpty(sortDirection))
        {
            sortDirection = "asc";
        }

        // Sort tasks if a sort criteria is provided.
        if (!string.IsNullOrEmpty(sortBy))
        {
            query = GetSorted(sortBy, sortDirection, query);
        }

        // Apply pagination to the query.
        query = GetPagedUnit(page, pageSize, query);

        // Execute the query and retrieve the list of ToDoTasks.
        var toDoTasks = await query.ToListAsync();

        return new PagedUnit<ToDoTask> { TotalCount = totalCount, Items = toDoTasks };
    }

    // Filters tasks by title.
    public IQueryable<ToDoTask> GetFiltered(string titleSearch, IQueryable<ToDoTask> query)
    {
        return query.Where(task => task.Title.ToLower().Contains(titleSearch.ToLower()));
    }

    // Sorts tasks based on the specified criteria and direction.
    public IQueryable<ToDoTask> GetSorted(string sortBy, string sortDirection, IQueryable<ToDoTask> query)
    {
        switch (sortBy.ToLower())
        {
            case "title":
                query = sortDirection.ToLower() == "desc" ? query.OrderByDescending(task => task.Title) : query.OrderBy(task => task.Title);
                break;
            case "duedate":
                query = sortDirection.ToLower() == "desc" ? query.OrderByDescending(task => task.DueDate) : query.OrderBy(task => task.DueDate);
                break;
            case "completed":
                query = sortDirection.ToLower() == "desc" ? query.OrderByDescending(task => task.Completed) : query.OrderBy(task => task.Completed);
                break;
            default:
                break;
        }
        return query;
    }

    // Applies pagination to the query based on the current page and page size.
    public IQueryable<ToDoTask> GetPagedUnit(int page, int pageSize, IQueryable<ToDoTask> query)
    {
        return query.Skip((page - 1) * pageSize).Take(pageSize);
    }

    // Creates a new ToDoTask in the database and returns the created task.
    public async Task<ToDoTask> CreateToDoTask(ToDoTask toDoTask)
    {
        // Ensure the task ID is set to zero for automatic ID generation.
        toDoTask.Id = 0;

        _context.ToDoTasks.Add(toDoTask);
        await _context.SaveChangesAsync();

        return toDoTask;
    }

    // Updates an existing ToDoTask in the database.
    public async Task UpdateToDoTask(ToDoTask toDoTask)
    {
        _context.Entry(toDoTask).State = EntityState.Modified;

        await _context.SaveChangesAsync();
    }

    // Deletes the specified ToDoTask from the database.
    public async Task DeleteToDoTask(ToDoTask toDoTask)
    {
        _context.ToDoTasks.Remove(toDoTask);
        await _context.SaveChangesAsync();
    }
}
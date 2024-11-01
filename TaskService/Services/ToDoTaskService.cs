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

    public async Task<ICollection<ToDoTask>> GetAll()
    {
        return await _context.ToDoTasks.AsNoTracking().ToListAsync();
    }

    public async Task<ToDoTask?> GetById(long id)
    {
        return await _context.ToDoTasks.FindAsync(id);
    }

    public async Task<PagedUnit<ToDoTask>> GetFiltered(string? titleSearch, string? sortBy, string? sortDirection, int page, int pageSize)
    {
        IQueryable<ToDoTask> query = _context.ToDoTasks;

        if (!string.IsNullOrEmpty(titleSearch))
        {
            query = query.Where(task => task.Title.ToLower().Contains(titleSearch.ToLower()));
        }

        if (string.IsNullOrEmpty(sortDirection))
        {
            sortDirection = "asc";
        }

        if (!string.IsNullOrEmpty(sortBy))
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
        }

        query = query.Skip((page - 1) * pageSize).Take(pageSize);

        var toDoTasks = await query.ToListAsync();

        return new PagedUnit<ToDoTask> { TotalCount = toDoTasks.Count, Items = toDoTasks };
    }

    public async Task<int> GetCount()
    {
        return await _context.ToDoTasks.CountAsync();
    }

    public async Task<ToDoTask> CreateToDoTask(ToDoTask toDoTask)
    {
        _context.ToDoTasks.Add(toDoTask);
        await _context.SaveChangesAsync();

        return toDoTask;
    }

    public async Task UpdateToDoTask(ToDoTask toDoTask)
    {
        _context.Entry(toDoTask).State = EntityState.Modified;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteToDoTask(ToDoTask toDoTask)
    {
        _context.ToDoTasks.Remove(toDoTask);
        await _context.SaveChangesAsync();
    }
}
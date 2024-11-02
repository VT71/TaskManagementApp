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

    public async Task<PagedUnit<ToDoTask>> GetByCriteria(string? titleSearch, string? sortBy, string? sortDirection, int page, int pageSize)
    {
        IQueryable<ToDoTask> query = _context.ToDoTasks;

        if (!string.IsNullOrEmpty(titleSearch))
        {
            query = GetFiltered(titleSearch, query);
        }

        int totalCount = await query.CountAsync();

        if (string.IsNullOrEmpty(sortDirection))
        {
            sortDirection = "asc";
        }

        if (!string.IsNullOrEmpty(sortBy))
        {
            query = GetSorted(sortBy, sortDirection, query);
        }

        query = GetPagedUnit(page, pageSize, query);

        var toDoTasks = await query.ToListAsync();

        return new PagedUnit<ToDoTask> { TotalCount = totalCount, Items = toDoTasks };
    }

    public IQueryable<ToDoTask> GetFiltered(string titleSearch, IQueryable<ToDoTask> query)
    {
        return query.Where(task => task.Title.ToLower().Contains(titleSearch.ToLower()));
    }

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

    public IQueryable<ToDoTask> GetPagedUnit(int page, int pageSize, IQueryable<ToDoTask> query)
    {
        return query.Skip((page - 1) * pageSize).Take(pageSize);
    }

    public async Task<ToDoTask> CreateToDoTask(ToDoTask toDoTask)
    {
        toDoTask.Id = 0;
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
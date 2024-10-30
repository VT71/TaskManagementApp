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

    public async Task<ICollection<ToDoTask>> GetFiltered(string criteria)
    {
        return await _context.ToDoTasks.Where(tak => tak.Title.ToLower().Contains(criteria.ToLower())).AsNoTracking().ToListAsync();
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
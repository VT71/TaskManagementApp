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
}
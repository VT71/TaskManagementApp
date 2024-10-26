using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskService.Models;
using TaskService.Data;

namespace TaskService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoTaskController : ControllerBase
    {
        private readonly ToDoTaskContext _context;

        public ToDoTaskController(ToDoTaskContext context)
        {
            _context = context;
        }

        // GET: api/ToDoTask
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoTask>>> GetToDoTasks()
        {
            return await _context.ToDoTasks.ToListAsync();
        }

        // GET: api/ToDoTask/5
        // [HttpGet("{id}")]
        // public async Task<ActionResult<ToDoTask>> GetToDoTask(long id)
        // {
        //     var toDoTask = await _context.ToDoTasks.FindAsync(id);

        //     if (toDoTask == null)
        //     {
        //         return NotFound();
        //     }

        //     return toDoTask;
        // }

        // PUT: api/ToDoTask/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutToDoTask(long id, ToDoTask toDoTask)
        // {
        //     if (id != toDoTask.Id)
        //     {
        //         return BadRequest();
        //     }

        //     _context.Entry(toDoTask).State = EntityState.Modified;

        //     try
        //     {
        //         await _context.SaveChangesAsync();
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         if (!ToDoTaskExists(id))
        //         {
        //             return NotFound();
        //         }
        //         else
        //         {
        //             throw;
        //         }
        //     }

        //     return NoContent();
        // }

        // POST: api/ToDoTask
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPost]
        // public async Task<ActionResult<ToDoTask>> PostToDoTask(ToDoTask toDoTask)
        // {
        //     _context.ToDoTasks.Add(toDoTask);
        //     await _context.SaveChangesAsync();

        //     return CreatedAtAction("GetToDoTask", new { id = toDoTask.Id }, toDoTask);
        // }

        // DELETE: api/ToDoTask/5
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteToDoTask(long id)
        // {
        //     var toDoTask = await _context.ToDoTasks.FindAsync(id);
        //     if (toDoTask == null)
        //     {
        //         return NotFound();
        //     }

        //     _context.ToDoTasks.Remove(toDoTask);
        //     await _context.SaveChangesAsync();

        //     return NoContent();
        // }

        // private bool ToDoTaskExists(long id)
        // {
        //     return _context.ToDoTasks.Any(e => e.Id == id);
        // }
    }
}

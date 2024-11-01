using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskService.Models;
using TaskService.Services;

namespace TaskService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoTaskController : ControllerBase
    {
        ToDoTaskService _service;

        public ToDoTaskController(ToDoTaskService service)
        {
            _service = service;
        }

        // GET: api/ToDoTask
        [HttpGet]
        public async Task<ActionResult<ICollection<ToDoTask>>> GetToDoTasks()
        {
            return Ok(await _service.GetAll());
        }

        // GET: api/ToDoTask/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoTask>> GetToDoTask(long id)
        {
            var toDoTask = await _service.GetById(id);

            if (toDoTask == null)
            {
                return NotFound();
            }

            return toDoTask;
        }

        [HttpGet("filter")]
        public async Task<ActionResult<ICollection<ToDoTask>>> GetFilteredToDoTasks(string? titleSearch, string? sortBy, string? sortDirection, int page = 1, int pageSize = 10)
        {
            return Ok(await _service.GetFiltered(titleSearch, sortBy, sortDirection, page, pageSize));
        }


        [HttpGet("count")]
        public async Task<ActionResult<ICollection<ToDoTask>>> GetToDoTasksCount()
        {
            return Ok(await _service.GetCount());
        }

        // POST: api/ToDoTask
        [HttpPost]
        public async Task<ActionResult<ToDoTask>> PostToDoTask(ToDoTask toDoTask)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ToDoTask newToDoTask = await _service.CreateToDoTask(toDoTask);

            return CreatedAtAction(nameof(GetToDoTask), new { id = newToDoTask.Id }, newToDoTask);
        }


        // PUT: api/ToDoTask/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutToDoTask(long id, ToDoTask toDoTask)
        {
            if ((id != toDoTask.Id) || !ModelState.IsValid)
            {
                return BadRequest();
            }

            await _service.UpdateToDoTask(toDoTask);

            return NoContent();
        }

        // DELETE: api/ToDoTask/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoTask(long id)
        {
            var toDoTask = await _service.GetById(id);
            if (toDoTask == null)
            {
                return NotFound();
            }

            await _service.DeleteToDoTask(toDoTask);

            return NoContent();
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskService.Models;
using TaskService.Services;

namespace TaskService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ToDoTaskController : ControllerBase
    {
        // Service for handling ToDoTask operations
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

            // Returns 404 if the task is not found
            if (toDoTask == null)
            {
                return NotFound();
            }

            return toDoTask;
        }

        // GET: api/ToDoTask/criteria
        [HttpGet("criteria")]
        public async Task<ActionResult<PagedUnit<ToDoTask>>> GetToDoTasksByCriteria(string? titleSearch, string? sortBy, string? sortDirection, int page = 1, int pageSize = 10)
        {
            return Ok(await _service.GetByCriteria(titleSearch, sortBy, sortDirection, page, pageSize));
        }

        // POST: api/ToDoTask
        [HttpPost]
        public async Task<ActionResult<ToDoTask>> PostToDoTask(ToDoTask toDoTask)
        {
            // Returns 400 if the model state is invalid
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
            // Returns 400 if the ID does not match or the model state is invalid
            if ((id != toDoTask.Id) || !ModelState.IsValid)
            {
                return BadRequest();
            }

            var searchedToDoTask = await _service.GetById(id);

            // Returns 404 if the task to update is not found
            if (searchedToDoTask == null)
            {
                return NotFound();
            }

            await _service.UpdateToDoTask(toDoTask);

            return NoContent();
        }

        // DELETE: api/ToDoTask/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoTask(long id)
        {
            var toDoTask = await _service.GetById(id);

            // Returns 404 if the task to delete is not found
            if (toDoTask == null)
            {
                return NotFound();
            }

            await _service.DeleteToDoTask(toDoTask);

            return NoContent();
        }
    }
}

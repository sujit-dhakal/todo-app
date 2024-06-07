using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Text;
using TodoApp.Data;
using TodoApp.Model;

namespace TodoApp.Controllers
{
    [ApiController] // to know it is an api contoller not a mvc controller
    [Route("api/[controller]")] // for routing
    public class TodoController : Controller
    {
        // dependency injection
        private readonly TodoContext _dbcontext;
        public TodoController(TodoContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todo>>> GetAllTodos() // to get all the todos use IEnumerable<Todo> or the Ok // aysnc returns Task<T>
        {
            return await _dbcontext.Todos.ToListAsync();  // if we use async await is necessary 
        }
        [HttpPost]
        public async Task<ActionResult<Todo>> PostTodo(AddTodo addtodo)
        {
            try
            {
                var todo = new Todo()
                {
                    Name = addtodo.Name,
                    IsComplete = addtodo.IsComplete,
                };
                if(todo == null)
                {
                    return BadRequest("Todo is null");
                }
                _dbcontext.Todos.Add(todo);
                await _dbcontext.SaveChangesAsync();
                return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, todo);
            }
            catch(Exception Ex)
            {
                return StatusCode(500, Ex.Message);
            }
        }
        [HttpGet("{id:long}")]
        public async Task<ActionResult<Todo>> GetTodo(long id)
        {
            try
            {
                var todo = await _dbcontext.Todos.FindAsync(id);
                if (todo == null)
                {
                    return NotFound();
                }
                return todo;
            }
            catch(Exception Ex)
            {
                return StatusCode(500, Ex.Message);
            }
        }
        [HttpPut("{id:long}")]
        public async Task<ActionResult<Todo>> UpdateTodo(long id, AddTodo todo)
        {
            try
            {
                var getTodo = await _dbcontext.Todos.FindAsync(id);
                if (getTodo != null)
                {
                    getTodo.Name = todo.Name;
                    getTodo.IsComplete = todo.IsComplete;
                    await _dbcontext.SaveChangesAsync();
                    return getTodo;
                }
                return NotFound();
            }
            catch (Exception Ex)
            {
                return StatusCode(500, Ex.Message);
            }
        }
        [HttpDelete("{id:long}")]
        public async Task<ActionResult<Todo>> DeleteTodo(long id)
        {
            try
            {
                var getTodo = await _dbcontext.Todos.FindAsync(id);
                if (getTodo == null)
                {
                    return NotFound();
                }
                _dbcontext.Todos.Remove(getTodo);
                await _dbcontext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception Ex)
            {
                return StatusCode(500, Ex.Message);
            }
        }

    }
}

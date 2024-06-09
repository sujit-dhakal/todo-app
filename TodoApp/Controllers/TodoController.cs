using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Text;
using TodoApp.Data;
using TodoApp.Model;
using TodoApp.Repositories;
using TodoApp.Services;

namespace TodoApp.Controllers
{
    [ApiController] // to know it is an api contoller not a mvc controller
    [Route("api/[controller]")] // for routing
    public class TodoController : Controller
    {
        // dependency injection
        private readonly ITodoService _todoservice;
        public TodoController(ITodoService service)
        {
            _todoservice = service;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todo>>> GetAllTodos()
        {
            return Ok(await _todoservice.GetAllTodos());
        }
        [HttpPost]
        public async Task<ActionResult<Todo>> PostTodo(AddTodo addtodo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // validation error 
            }
            try
            {
                return await _todoservice.PostTodo(addtodo);
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
                return await _todoservice.GetTodo(id);
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception Ex)
            {
                return StatusCode(500, Ex.Message);
            }
        }
        [HttpPut("{id:long}")]
        public async Task<ActionResult<Todo>> UpdateTodo(long id, AddTodo addtodo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // validation error
            }
            try
            {
                return await _todoservice.UpdateTodo(id, addtodo);
            }
            catch(NotFoundException ex)
            {
                return BadRequest(ex.Message);
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
                return await _todoservice.DeleteTodo(id);
            }
            catch (Exception Ex)
            {
                return StatusCode(500, Ex.Message);
            }
        }

    }
}

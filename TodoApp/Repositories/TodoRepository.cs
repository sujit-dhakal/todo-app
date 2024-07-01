using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Model;
using TodoApp.Services;

namespace TodoApp.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly TodoContext _context;
        private readonly IRabbitMQService _rabbitmqService;
        public TodoRepository(TodoContext context,IRabbitMQService rabbitmqservice)
        {
            _context = context;
            _rabbitmqService = rabbitmqservice;
        }
        public async Task<IEnumerable<Todo>> GetAllTodos() => await _context.Todos.OrderBy(todo=>todo.Id).ToListAsync();
        public async Task<Todo> GetTodo(long id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
            {
                throw new NotFoundException("Todo not found");
            }
            return todo;
        }
        public async Task<Todo> PostTodo(AddTodo addtodo)
        {
            var todo = new Todo()
            {
                Name = addtodo.Name,
                IsComplete = addtodo.IsComplete,
                CreatedAt = DateTime.UtcNow,
            };
            await _context.Todos.AddAsync(todo);
            await SaveChanges();
            return todo;
    }
        public async Task<Todo> UpdateTodo(long id, AddTodo addtodo)
        {
            var gettodo = await GetTodo(id);
            gettodo.Name = addtodo.Name;
            gettodo.IsComplete = addtodo.IsComplete;
            await SaveChanges();
            return gettodo;
        }
        public async Task<Todo> DeleteTodo(long id)
        {
            var gettodo = await GetTodo(id);
            _context.Todos.Remove(gettodo);
            await SaveChanges();
            return gettodo;
        }

        public async Task SaveChanges() => await _context.SaveChangesAsync();
    }
}

using Microsoft.AspNetCore.Http.HttpResults;
using TodoApp.Model;
using TodoApp.Repositories;

namespace TodoApp.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _todorepository;
        public TodoService(ITodoRepository todorepository)
        {
            _todorepository = todorepository;
        }
        public async Task<IEnumerable<Todo>> GetAllTodos() => await _todorepository.GetAllTodos();

        public async Task<Todo> GetTodo(long id) => await _todorepository.GetTodo(id);
        public async Task<Todo> PostTodo(AddTodo addtodo)=> await _todorepository.PostTodo(addtodo);
        public async Task<Todo> UpdateTodo(long id, AddTodo addtodo) => await  _todorepository.UpdateTodo(id, addtodo);
        public async Task<Todo> DeleteTodo(long id) => await _todorepository.DeleteTodo(id);
    }
}

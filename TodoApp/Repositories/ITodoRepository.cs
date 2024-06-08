﻿using TodoApp.Model;

namespace TodoApp.Repositories
{
    public interface ITodoRepository
    {
        Task<IEnumerable<Todo>> GetAllTodos();
        Task<Todo> PostTodo(AddTodo addtodo);
        Task<Todo> GetTodo(long id);
        Task<Todo> UpdateTodo(long id, AddTodo todo);
        Task<Todo> DeleteTodo(long id);
        Task SaveChanges();
    }
}

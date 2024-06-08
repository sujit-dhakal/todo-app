using TodoApp.Model;

namespace TodoApp.Repositories
{
    // interface for User
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User>Register(AddUser adduser);
        Task<User> Login(AddUser adduser);
    }
}

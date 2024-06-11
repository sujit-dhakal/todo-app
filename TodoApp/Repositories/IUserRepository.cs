using TodoApp.Model;

namespace TodoApp.Repositories
{
    // interface for User
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User>Register(AddUser adduser);
        Task<TokenResponse> Login(LoginUser loginuser);
        Task<TokenResponse> RefreshAccessToken(long Id);
    }
}

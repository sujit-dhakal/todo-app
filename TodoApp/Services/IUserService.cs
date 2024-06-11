using TodoApp.Model;

namespace TodoApp.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> Register(AddUser adduser);
        Task<TokenResponse> Login(LoginUser loginuser);
        Task<TokenResponse> RefreshAccessToken(long Id);
    }
}

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Text;
using TodoApp.Data;
using TodoApp.Model;

namespace TodoApp.Repositories
{
    // repository are for encapsulating the data access code
    public class UserRepository : IUserRepository
    {
        private readonly TodoContext _context;
        public UserRepository(TodoContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<User>> GetUsers() => await _context.Users.ToListAsync();
        public async Task<User> Register(AddUser adduser)
        {
            var getuser = await _context.Users.FirstOrDefaultAsync(p=>p.Username == adduser.Username);
            if (getuser != null)
            {
                throw new UserNameAlreadyExists("username already taken try a new one.");
            }
            string passwordhash = BCrypt.Net.BCrypt.HashPassword(adduser.Password);
            var user = new User() { 
                Username = adduser.Username,
                Password = passwordhash,
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<User> Login(AddUser adduser)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == adduser.Username);
            if (user == null)
            {
                throw new NotFoundException("user not found");
            }
            bool verified = BCrypt.Net.BCrypt.Verify(adduser.Password, user.Password);
            if (!verified)
            {
                throw new InvalidPasswordException("invalid password");
            }
            return user;
        }
    }
}

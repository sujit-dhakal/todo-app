using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Model;

namespace TodoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly TodoContext _dbcontext;
        public UserController(TodoContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _dbcontext.Users.ToListAsync();
        }
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(AddUser adduser)
        {
            var user = await _dbcontext.Users.FirstOrDefaultAsync(u=>u.Username == adduser.Username);
            if(user != null)
            {
                return BadRequest("username already exists try a different one");   
            }
            string passwordhash = BCrypt.Net.BCrypt.HashPassword(adduser.Password);
            try
            {
                var useradd = new User() { 
                    Username = adduser.Username,
                    Password = passwordhash
                };
                _dbcontext.Users.Add(useradd);
                await _dbcontext.SaveChangesAsync();
                return Ok();
            }
            catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }
        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(AddUser adduser)
        {
            try
            {
                var user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Username == adduser.Username);
                if (user == null)
                {
                    return BadRequest("user not found");
                }
                bool verified = BCrypt.Net.BCrypt.Verify(adduser.Password, user.Password);
                if (!verified)
                {
                    return Unauthorized("invalid password");
                }
                return Ok(user);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

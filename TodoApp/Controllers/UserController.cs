using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Model;
using TodoApp.Repositories;
using TodoApp.Services;

namespace TodoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userservice;
        public UserController(IUserService userservice)
        {
            _userservice = userservice;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return Ok(await _userservice.GetUsers());
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(AddUser adduser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  // validation error
            }
            try
            {
                return await _userservice.Register(adduser);
            }
            catch(UserNameAlreadyExists ex)
            {
                return BadRequest(ex.Message);
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
               return await _userservice.Login(adduser);
            }
            catch(InvalidPasswordException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

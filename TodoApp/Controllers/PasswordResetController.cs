using Microsoft.AspNetCore.Mvc;
using TodoApp.Model;
using TodoApp.Services;

namespace TodoApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PasswordResetController : Controller
    {
        private readonly IPasswordResetService _passwordResetService;
        public PasswordResetController(IPasswordResetService passwordResetService)
        {
            _passwordResetService = passwordResetService;
        }
        [HttpPost("request")]
        public async Task<IActionResult> RequestPasswordReset(string Email)
        {
            var token = _passwordResetService.GeneratePasswordResetToken();
            var expiryTime = DateTime.UtcNow.AddMinutes(5);
            await _passwordResetService.StorePasswordResetToken(Email,token, expiryTime);
            await _passwordResetService.SendPasswordResetEmail(Email,token);
            return Ok("Password reset link has been sent to your email");
        }
        [HttpPost("reset")]
        public async Task<IActionResult> ResetPassword(ResetModel model)
        {
            await _passwordResetService.ResetPasswordAsync(model.Token,model.NewPassword);
            return Ok("Password has been reset successfully");
        }

    }
}

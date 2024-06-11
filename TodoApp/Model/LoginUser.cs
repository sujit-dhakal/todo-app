using System.ComponentModel.DataAnnotations;

namespace TodoApp.Model
{
    public class LoginUser
    {
        [Required(ErrorMessage = ("Email is required"))]
        [EmailAddress(ErrorMessage = ("Enter a valid email address"))]
        public required string Email { get; set; }

        public string Password { get; set; }
    }
}

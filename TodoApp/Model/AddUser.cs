using System.ComponentModel.DataAnnotations;

namespace TodoApp.Model
{
    public class AddUser
    {
        [Required(ErrorMessage =("Username is required"))]
        [StringLength(100,MinimumLength = 5, ErrorMessage ="username should 5 or more characters long")]
        public required string Username { get; set; }

        [Required(ErrorMessage =("Password is required"))]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "password should be 8 or more characters long")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#])[A-Za-z\d@$!%*?&#]{8,}$", ErrorMessage = "Password must have at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public required string Password { get; set; }
    }
}

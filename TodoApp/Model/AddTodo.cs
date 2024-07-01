using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TodoApp.Model
{
    public class AddTodo
    {
        [Required(ErrorMessage ="Name is required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage ="Name must be 5 or more characters long")]
        public string? Name { get; set; }
        public bool IsComplete { get; set; } = false;
        public DateTime CreatedAt { get; set; }
    }
}

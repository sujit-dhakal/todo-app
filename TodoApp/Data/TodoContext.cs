
using Microsoft.EntityFrameworkCore;
using TodoApp.Model;

namespace TodoApp.Data
{
        public class TodoContext : DbContext
        {
            public TodoContext(DbContextOptions<TodoContext> options) : base(options) { }
            public DbSet<Todo> Todos { get; set; } = null!;
            public DbSet<User> Users { get; set; }
            public DbSet<PasswordResetModel> PasswordResetModels { get; set; }

            //ModelBuilder is used to configure our database schema
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                // for unique username 
                modelBuilder.Entity<User>()
                    .HasIndex(u => u.Username)
                    .IsUnique();
            }
    }
}

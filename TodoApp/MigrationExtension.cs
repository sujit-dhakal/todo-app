using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using TodoApp.Data;

namespace TodoApp
{
    public static class MigrationExtension
    {
        public static void ApplyMigration(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            using TodoContext todoContext = scope.ServiceProvider.GetRequiredService<TodoContext>();
            todoContext.Database.Migrate();
        }
    }
}

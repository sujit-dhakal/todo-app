using dotenv.net;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TodoApp;
using TodoApp.CustomMiddleWare;
using TodoApp.Data;
using TodoApp.Repositories;
using TodoApp.Services;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);


var conn = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.

builder.Services.AddControllers();
// register the dbcontext context
builder.Services.AddDbContext<TodoContext>(options => options.UseNpgsql(conn));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// IOC container
builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddScoped<ITodoService, TodoService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordResetRepository, PasswordResetRepository>();
builder.Services.AddScoped<IPasswordResetService, PasswordResetService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "reactApp", configurePolicy: policyBuilder =>
    {
        policyBuilder.WithOrigins("http://localhost:5173");
        policyBuilder.AllowAnyHeader();
        policyBuilder.AllowAnyMethod();
        policyBuilder.AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
app.ApplyMigration();
//}

// middleware for 500 error
app.UseErrorHandlingMiddleware();

//app.UseHttpsRedirection();
app.UseCors("reactApp");

app.UseAuthorization();

app.MapControllers();

app.Run();

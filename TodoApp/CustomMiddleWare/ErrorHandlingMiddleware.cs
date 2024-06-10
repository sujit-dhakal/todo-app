using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TodoApp.CustomMiddleWare
{
    public class ErrorHandlingMiddleware
    {
        // for refrencing nexte middleware in the pipeline
        private readonly RequestDelegate _next;
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        // this method is called for each Http request
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync("An unexpected error occured." + ex.Message);
            }
        }
    }
    public static class ErrorHandlingMiddlewareExtensions
    {
        // extension method that allows adding the error handling middleware to the middleware pipleline
        public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }

}


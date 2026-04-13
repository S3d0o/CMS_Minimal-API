using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CMS.CustomMiddleWares
{
    public class GlobalExceptionHandlingMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public GlobalExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionHandlingMiddleware> logger,
            IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); 

                // Handle 404 only if no endpoint matched
                if (context.Response.StatusCode == StatusCodes.Status404NotFound &&
                    context.GetEndpoint() == null &&
                    !context.Response.HasStarted)
                {
                    await HandleNotFoundAsync(context);
                }
            }
            catch (Exception ex)
            {
                // if the response has already started, we can't modify it, so we just log the error and return
                if (context.Response.HasStarted) 
                    return;

                _logger.LogError(ex,
                    "Unhandled exception. Path: {Path}, TraceId: {TraceId}",
                    context.Request.Path,
                    context.TraceIdentifier);
               
                await HandleExceptionAsync(context, ex);
            }
        }

        // this method creates a ProblemDetails response with appropriate details based on the environment
        // (development or production) and writes it to the response stream as JSON.
        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            var statusCode = context.Response.StatusCode = ex switch
            {
                // you can add custom exceptions here and return appropriate status codes based on the exception type
                (_) => StatusCodes.Status500InternalServerError
            };

            var problem = new ProblemDetails
            {
                Title = "Internal Server Error",
                Status = statusCode,
                Detail = _env.IsDevelopment()
                    ? ex.Message
                    : "An unexpected error occurred.",
                Instance = context.Request.Path
            };

            // include the stack trace in development environment for easier debugging
            problem.Extensions["TraceId"] =
                Activity.Current?.Id ?? context.TraceIdentifier;

            await context.Response.WriteAsJsonAsync(problem);
        }

        // this method creates a ProblemDetails response for 404 Not Found errors and writes it to the response stream as JSON.
        private async Task HandleNotFoundAsync(HttpContext context)
        {
            var problem = new ProblemDetails
            {
                Title = "Endpoint Not Found",
                Status = 404,
                Detail = $"No endpoint matches {context.Request.Path}",
                Instance = context.Request.Path
            };

            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}


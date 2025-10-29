using Serilog;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace UserManagement.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Log the request path at the start of InvokeAsync, where 'context' is available
            Log.Information(" ExceptionMiddleware executing for: {Path}", context.Request.Path);

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Serilog logs directly here — no need for ILogger
                Log.Error(ex, "Unhandled exception caught in middleware: {Message}", ex.Message);

                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            var response = context.Response;
            var result = string.Empty;

            switch (ex)
            {
                case ValidationException ve:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(new
                    {
                        message = ve.Message,
                        errors = ve.Errors
                    });
                    Log.Warning("Validation error: {Errors}", ve.Errors);
                    break;

                case KeyNotFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    result = JsonSerializer.Serialize(new { message = ex.Message });
                    Log.Warning("Not Found: {Message}", ex.Message);
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.BadRequest; // or InternalServerError if you prefer
                    result = JsonSerializer.Serialize(new
                    {
                        message = ex.Message
                    });
                    Log.Error(ex, "Unhandled exception: {Message}", ex.Message);
                    break;
            }

            await context.Response.WriteAsync(result);
        }
    }
}
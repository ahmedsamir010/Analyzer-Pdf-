using System.Net;
using System.Text.Json;

namespace API.Errors
{
    public class ExceptionMiddleware(RequestDelegate next, IHostEnvironment hostEnvironment)
    {
        private readonly RequestDelegate _next = next;
        private readonly IHostEnvironment _hostEnvironment = hostEnvironment;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            var response = _hostEnvironment.IsDevelopment()
                ? new ApiExceptionResponse((int)code, exception.Message, exception.StackTrace ?? string.Empty)
                : new ApiExceptionResponse((int)code);

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}


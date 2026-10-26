using System.Net;
using System.Text.Json;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (UnauthorizedAccessException ex)
            {
                await HandleExceptionAsync(
                    context,
                    HttpStatusCode.Unauthorized,
                    ex.Message
                );
            }
            catch (KeyNotFoundException ex)
            {
                await HandleExceptionAsync(
                    context,
                    HttpStatusCode.NotFound,
                    ex.Message
                );
            }
            catch (ArgumentException ex)
            {
                await HandleExceptionAsync(
                    context,
                    HttpStatusCode.BadRequest,
                    ex.Message
                );
            }
            catch (InvalidOperationException ex)
            {
                await HandleExceptionAsync(
                    context,
                    HttpStatusCode.Conflict,
                    ex.Message
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error no controlado en la aplicación."
                );

                await HandleExceptionAsync(
                    context,
                    HttpStatusCode.InternalServerError,
                    "Ocurrió un error interno en el servidor."
                );
            }
        }

        private static async Task HandleExceptionAsync(
            HttpContext context,
            HttpStatusCode statusCode,
            string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                status = (int)statusCode,
                message
            };

            var json = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
    }
}
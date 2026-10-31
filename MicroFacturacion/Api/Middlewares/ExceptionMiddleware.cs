using Core.Facturacion.Exceptions;
using Newtonsoft.Json;

namespace API.Middlewares
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
            catch (DomainException ex)
            {
                _logger.LogWarning(
                    "Error de dominio {Codigo}: {Mensaje}",
                    (int)ex.Error,
                    ex.Message
                );

                context.Response.StatusCode =
                    StatusCodes.Status400BadRequest;

                context.Response.ContentType =
                    "application/json";

                var response = new
                {
                    codigo = (int)ex.Error,
                    error = ex.Error.ToString(),
                    mensaje = ex.Message
                };

                await context.Response.WriteAsync(
                    JsonConvert.SerializeObject(response)
                );
            }
            catch (ArgumentException ex)
            {
                context.Response.StatusCode =
                    StatusCodes.Status400BadRequest;

                context.Response.ContentType =
                    "application/json";

                var response = new
                {
                    codigo = 400,
                    error = "SOLICITUD_INVALIDA",
                    mensaje = ex.Message
                };

                await context.Response.WriteAsync(
                    JsonConvert.SerializeObject(response)
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error interno no controlado."
                );

                context.Response.StatusCode =
                    StatusCodes.Status500InternalServerError;

                context.Response.ContentType =
                    "application/json";

                var response = new
                {
                    codigo = 500,
                    error = "ERROR_INTERNO",
                    mensaje = "Ocurrió un error interno en el servidor."
                };

                await context.Response.WriteAsync(
                    JsonConvert.SerializeObject(response)
                );
            }
        }
    }
}
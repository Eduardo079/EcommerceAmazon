using System.Net;
using Ecommerce.Api.Errors;
using Ecommerce.Application.Exceptions;
using Newtonsoft.Json;
using SendGrid.Helpers.Errors.Model;
using BadRequestException = Ecommerce.Application.Exceptions.BadRequestException;
using NotFoundException = Ecommerce.Application.Exceptions.NotFoundException;

namespace Ecommerce.Api.Middleware;

/// <summary>
/// Middleware que intercepta excepciones no controladas durante el procesamiento de una petición HTTP,
/// registra el error y devuelve una respuesta JSON consistente con el código HTTP apropiado.
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;     // Delegado al siguiente middleware en la tubería.

    private readonly ILogger<ExceptionMiddleware> _logger;  // Delegado al siguiente middleware en la tubería.

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;       // Guarda la referencia al siguiente middleware.
        _logger = logger;   // Guarda el logger.
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            // Deja pasar la petición al siguiente middleware/endpoint.
            await _next(httpContext);
        }
        catch (Exception ex)    
        {
            _logger.LogError(ex, ex.Message);   // Captura cualquier excepción no controlada.
            httpContext.Response.ContentType = "application/json";  // Fuerza que la respuesta sea JSON.
            var statusCode = (int)HttpStatusCode.InternalServerError;   // Por defecto, 500 Internal Server Error.
            var result = string.Empty;   // Aquí se almacenará el JSON final a devolver.

            // Mapea el tipo de excepción a un status code y/o payload específico.
            switch (ex)
            {
                case NotFoundException notFoundException:    // Si es una excepción de "no encontrado" propia de la app...
                    statusCode = (int)HttpStatusCode.NotFound;  // ...responder con 404 Not Found.
                    break;

                // Errores de validación (FluentValidation) → 400.
                case FluentValidation.ValidationException validationException:  
                    statusCode = (int)HttpStatusCode.BadRequest;
                    // Recorre cada ValidationFailure...
                    var errors = validationException.Errors.
                    // ...y toma solamente el mensaje de error (Nota: el parámetro 'errors' de la lambda sombrea el nombre de la variable).
                    Select(errors => errors.ErrorMessage)
                    // Convierte la secuencia a arreglo de strings.
                    .ToArray();

                    // Serializa el arreglo de mensajes (string[] → JSON).
                    var ValidationJsons = JsonConvert.SerializeObject(errors);
                    // Construye el cuerpo de respuesta como JSON...
                    result = JsonConvert.SerializeObject(
                        // ...usando CodeErrorsException: status, mensajes y detalles (aquí, el JSON de validaciones).
                        new CodeErrorsException(statusCode, errors,
                        ValidationJsons));
                    break;
                case BadRequestException badRequestException: // Petición incorrecta definida por la app → 400.
                    statusCode = (int)HttpStatusCode.BadRequest;
                    break;
                default:    // Cualquier otra excepción no contemplada → 500.
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            // Si no se construyó un JSON específico en el switch (ej. 404/400 simples)...
            if (string.IsNullOrEmpty(result))
            {
                // ...construye un JSON genérico con el mensaje de la excepción...
                result = JsonConvert.SerializeObject(
                    new CodeErrorsException(
                        statusCode,// ...incluyendo el status calculado...
                        new string[] { ex.Message }, // ...un arreglo con el mensaje de error...
                        ex.StackTrace));     // ...y los detalles (stack trace) de la excepción.
            }

            // Escribe el código HTTP de la respuesta (404/400/500, etc.).
            httpContext.Response.StatusCode = statusCode;
            // Escribe el JSON en el body de la respuesta y finaliza.
            await httpContext.Response.WriteAsync(result);
        }
    }
}
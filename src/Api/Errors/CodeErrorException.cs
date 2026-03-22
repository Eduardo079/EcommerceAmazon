using Newtonsoft.Json;

namespace Ecommerce.Api.Errors;

public class CodeErrorsException : CodeErrorsResponse   // Declara una clase que HEREDA de CodeErrorsResponse (extiende ese DTO).
{
    [JsonProperty(PropertyName = "details")]            // Indica que, al serializar con Newtonsoft, esta propiedad se llamará "details" en el JSON.
    public string? Details { get; set; }                // Propiedad opcional con detalles adicionales del error (por ejemplo, trazas o info extra).
    
    public CodeErrorsException(int statusCode, string[]? message = null, string? details = null) // Constructor con status, mensajes y detalles opcionales.
    : base(statusCode, message)     // Llama al constructor de la clase base (CodeErrorsResponse) para inicializar StatusCode y Message.
    {
        Details = details;          // Asigna el valor recibido a la propiedad Details de esta clase derivada.
    }
}
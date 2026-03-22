using System.Text.Json;
using Azure.Core.Serialization;
using Newtonsoft.Json;

namespace Ecommerce.Api.Errors;

public class CodeErrorsResponse // DTO para representar una respuesta de error en la API.
{
    [JsonProperty(PropertyName = "statusCode")]     // Indica que al serializar con Newtonsoft el nombre será "statusCode".
    public int StatusCode { get; set; }             // Código de estado HTTP que se incluirá dentro del JSON (dato informativo).

    [JsonProperty(PropertyName = "message")]        // Indica que al serializar con Newtonsoft el nombre será "message".
    public string[]? Message { get; set; }          // Arreglo (opcional) de mensajes de error a devolver en la respuesta.

    public CodeErrorsResponse(int statusCode, String[]? message = null)   // Constructor que recibe el status y opcionalmente los mensajes.
    {
        StatusCode = statusCode;        // Asigna el código de estado a la propiedad.
        if (message is null)            // Si no se proporcionaron mensajes…
        {
            Message = new string[0];    // Inicializa el arreglo de mensajes como un arreglo de longitud 0.
            var text = GetDefaultMessageStatusCode(statusCode); // Obtiene un mensaje por defecto según el status.
            Message[0] = text;          // Intenta asignar el texto en la primera posición del arreglo.
                                        // (Comentario: esto escribe en índice 0 de un arreglo de tamaño 0)
        }
        else                            // Si sí se proporcionaron mensajes…
        {
            Message = message;          // Usa los mensajes que llegaron por parámetro.
        }
    }

    private string GetDefaultMessageStatusCode(int statusCode)   // Método privado que devuelve un mensaje por defecto según el código.
    {
        return statusCode switch                                 // Expresión switch para mapear códigos a textos.
        {
            400 => "El request enviado tiene errores",          // Mensaje por defecto para 400 (Bad Request).
            401 => "No tienes Authorization para este recurso", // Mensaje por defecto para 401 (Unauthorized).
            404 => "No se encontro el recurso solicitado",      // Mensaje por defecto para 404 (Not Found).
            500 => "Se produjeron errores en el servidor",      // Mensaje por defecto para 500 (Internal Server Error).
            _ => string.Empty                                   // Para otros códigos, devuelve cadena vacía.
        };
    }
}
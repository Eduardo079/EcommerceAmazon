using System.Runtime.Serialization;

namespace Ecommerce.Domain;

public enum OrderStatus
{
    [EnumMember(Value = "Pendiente")]
    pending,
    [EnumMember(Value = "El Pago Fue Recibido")]
    Completed,
    [EnumMember(Value = "El Producto FUe Enviado al CLiente")]
    Enviado,
    [EnumMember(Value = "El Pago tuvo errores")]
    Error


}
using System.ComponentModel.DataAnnotations.Schema;
using Ecommerce.Domain.Common;

namespace Ecommerce.Domain;

public class Order : BaseDomainModel
{
    public Order() {}
    public Order(string compradorName,
    string compradorNameEmail, OrderAddress orderAddress,
    decimal subTotal, decimal total, decimal impuesto,
    decimal precioEnvio)
    {
        CompradorName = compradorName;
        CompradorUserName = compradorNameEmail;
        OrderAddress = orderAddress;
        SubTotal = subTotal;
        Total = total;
        Impuesto = impuesto;
        PrecioEnvio = precioEnvio;
    }

    public string? CompradorName { get; set; }
    public string? CompradorUserName { get; set; }
    public OrderAddress OrderAddress { get; set; }
    public IReadOnlyList<OrderItem> OrderItems { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal SubTotal { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.pending;

    [Column(TypeName = "decimal(10,2)")]
    public decimal Total { get; set; }
    [Column(TypeName = "decimal(10,2)")]
    public decimal Impuesto { get; set; }
    [Column(TypeName = "decimal(10,2)")]
    public decimal PrecioEnvio { get; set; }
    public string? PaymentIntentId { get; set; }
    public string? ClientSecret { get; set; }
    public string? StripApiKey { get; set; }
    
}
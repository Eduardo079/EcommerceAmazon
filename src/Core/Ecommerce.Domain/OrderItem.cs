using System.ComponentModel.DataAnnotations.Schema;
using Ecommerce.Domain.Common;

namespace Ecommerce.Domain;

public class OrderItem : BaseDomainModel
{
    public Product? Product { set; get; }
    public int ProductId { get; set; }

    
    public decimal Precio { get; set; }
    public int Cantidad { get; set; }
    public Order? Order { get; set; }
    public int ProductItemId { get; set; }
    public string? ProductName { get; set; }
    public string? ImagenUrl { get; set; }
}
using Ecommerce.Domain.Common;

namespace Ecommerce.Domain;

public class ShoppinCart : BaseDomainModel
{
    public Guid? ShoppingCartMasrterId { get; set; }
    public virtual ICollection<ShoppingCartItem>? ShoppingCartItems { get; set; }
}
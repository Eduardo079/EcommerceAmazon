using Ecommerce.Domain;
using MediatR;

namespace Ecommerce.Application.Feature.Products.Queries.GetProductList;
public class GetProductListQuery : IRequest<IReadOnlyList<Product>>
{
    
}
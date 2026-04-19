using Ecommerce.Application.Features.Products.Queries.PaginationProducts;
using Ecommerce.Application.Features.Products.Queries.Vms;
using Ecommerce.Application.Features.Shared.Queries;
using MediatR;

namespace Ecommerce.Application.Features.Products.Queries.PaginationProductsÑ

public class PaginationProductQueryHandler : IRequestHandler<PaginationProductQuery, PaginationVm<ProductVm>>
{
    public Task<PaginationVm<ProductVm>> Handle(PaginationProductQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

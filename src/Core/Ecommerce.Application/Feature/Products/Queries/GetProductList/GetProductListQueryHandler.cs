using System.Linq.Expressions;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;
using MimeKit.Encodings;

namespace Ecommerce.Application.Feature.Products.Queries.GetProductList;

public class GetProductListQueryHandler : IRequestHandler<GetProductListQuery, List<Product>>
{
    private readonly IUnitOfWork _iunitOfWork;

    public GetProductListQueryHandler(IUnitOfWork iunitOfWork)
    {
        _iunitOfWork = iunitOfWork;
    }

    public async Task<List<Product>> Handle(GetProductListQuery request, CancellationToken cancellationToken)
    {
        List<Expression<Func<Product, object>>> includes = new List<Expression<Func<Product, object>>>();
        includes.Add(P  => P.Images!);
        includes.Add(P => P.Reviews!);

        IReadOnlyList<Product> products = await  _iunitOfWork.Repository<Product>().GetAsync(
            null,
            x => x.OrderBy(y => y.Nombre),
            includes,
            true
        );
        
        return new List<Product>(products);
    }
}
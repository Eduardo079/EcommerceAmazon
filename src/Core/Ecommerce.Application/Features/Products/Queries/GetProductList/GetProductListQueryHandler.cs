using System.Linq.Expressions;
using AutoMapper;
using Ecommerce.Application.Features.Products.Queries.Vms;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;
using MimeKit.Encodings;

namespace Ecommerce.Application.Features.Products.Queries.GetProductList;

public class GetProductListQueryHandler : IRequestHandler<GetProductListQuery, IReadOnlyList<ProductVm>>
{
    private readonly IUnitOfWork _iunitOfWork;
    private readonly IMapper _mapper;

    public GetProductListQueryHandler(IUnitOfWork iunitOfWork, IMapper mapper)
    {
        _iunitOfWork = iunitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<ProductVm>> Handle(GetProductListQuery request, CancellationToken cancellationToken)
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
        IReadOnlyList<ProductVm> productsVm = _mapper.Map<IReadOnlyList<ProductVm>>(products);
        
        return productsVm;
    }
}
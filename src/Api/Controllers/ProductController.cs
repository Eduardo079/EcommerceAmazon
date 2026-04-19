using System.Net;
using Ecommerce.Application.Features.Products.Queries.GetProductList;
using Ecommerce.Application.Features.Products.Queries.Vms;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers;
[ApiController]
[Route("api/v1/[controller]")]
public class ProductController : ControllerBase
{

    private readonly IMediator _mediator;

    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpGet("list", Name = "GetProdutList")]
    [ProducesResponseType(typeof(IReadOnlyList<ProductVm>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IReadOnlyList<ProductVm>>> GetProdutList()
    {
        GetProductListQuery query = new GetProductListQuery();
        IReadOnlyList<ProductVm> products = await _mediator.Send(query);
        return Ok(products);
    }
}
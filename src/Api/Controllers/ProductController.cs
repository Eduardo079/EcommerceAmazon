using System.Net;
using Ecommerce.Application.Feature.Products.Queries.GetProductList;
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
    [ProducesResponseType(typeof(IReadOnlyList<Product>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProdutList()
    {
        GetProductListQuery query = new GetProductListQuery();
        IReadOnlyList<Product> products = await _mediator.Send(query);
        return Ok(products);
    }
}
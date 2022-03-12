using HepsiFly.Api.Attributes;
using HepsiFly.Api.Middlewares;
using HepsiFly.Business.Products.Commands.CreateProduct;
using HepsiFly.Business.Products.Commands.DeleteProduct;
using HepsiFly.Business.Products.Commands.UpdateProduct;
using HepsiFly.Business.Products.Queries.GetProductById;
using HepsiFly.Business.Products.Queries.GetProductsByFilter;
using HepsiFly.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HepsiFly.Api.Controllers;

[Route("api/products")]
[ApiController]
[Produces("application/json")]
public class ProductsController : ControllerBase
{
    private readonly ILogger<ProductsController> _logger;
    private readonly IMediator _mediator;

    public ProductsController(
        ILogger<ProductsController> logger,
        IMediator mediator
    )
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost]
    [ValidateModelState]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command)
    {
        var product = await _mediator.Send(command);
        
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    [ValidateModelState]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateProduct([BindRequired] string id, [FromBody] UpdateProductCommand command)
    {
        command.Id = id;
        await _mediator.Send(command);
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteProduct([BindRequired] string id)
    {
        await _mediator.Send(new DeleteProductCommand
        {
            Id = id
        });

        return NoContent();
    }

    [HttpGet("list")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(List<Product>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByName([BindRequired] string name)
    {
        var productList = await _mediator.Send(new GetProductsByFilterQuery()
        {
           Name = name
        });
        
        return Ok(productList);
    }
    
    [HttpGet("{id}",Name = "GetById")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetById([BindRequired] string id)
    {
        var product = await _mediator.Send(new GetProductByIdQuery
        {
            Id = id
        });
        
        return Ok(product);
    }
}
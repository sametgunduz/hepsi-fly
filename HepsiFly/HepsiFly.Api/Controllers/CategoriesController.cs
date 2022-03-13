using HepsiFly.Api.Attributes;
using HepsiFly.Api.Middlewares;
using HepsiFly.Business.Categories.Commands.CreateCategory;
using HepsiFly.Business.Categories.Commands.DeleteCategory;
using HepsiFly.Business.Categories.Commands.UpdateCategory;
using HepsiFly.Business.Categories.Queries.GetCategoriesByFilter;
using HepsiFly.Business.Categories.Queries.GetCategoryById;
using HepsiFly.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace HepsiFly.Api.Controllers;

[Route("api/categories")]
[ApiController]
[Produces("application/json")]
public class CategoriesController : ControllerBase
{
    private readonly ILogger<CategoriesController> _logger;
    private readonly IMediator _mediator;

    public CategoriesController(
        ILogger<CategoriesController> logger,
        IMediator mediator
    )
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Category), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryCommand command)
    {
        var category = await _mediator.Send(command);
        
        return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
    }

    [HttpPut("{id}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateCategory([BindRequired] string id, [FromBody] UpdateCategoryCommand command)
    {
        command.Id = id;
        await _mediator.Send(command);
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteCategory([BindRequired] string id)
    {
        await _mediator.Send(new DeleteCategoryCommand
        {
            Id = id
        });

        return NoContent();
    }

    [HttpGet("list")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(List<Category>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByName([BindRequired] string name)
    {
        var categoryList = await _mediator.Send(new GetCategoriesByFilterQuery()
        {
           Name = name
        });
        
        return Ok(categoryList);
    }
    
    [HttpGet("{id}",Name = "GetById")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Category), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetById([BindRequired] string id)
    {
        var category = await _mediator.Send(new GetCategoryByIdQuery
        {
            Id = id
        });
        
        return Ok(category);
    }
}
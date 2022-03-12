using HepsiFly.Business.Categories.Queries.GetCategoryById;
using HepsiFly.Common.Exceptions;
using HepsiFly.Domain.Contracts;
using HepsiFly.Domain.Entities;
using MediatR;

namespace HepsiFly.Business.Products.Queries.GetProductById;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Product>
{
    private readonly IProductRepository _repository;
    private readonly IMediator _mediator;

    public GetProductByIdQueryHandler(IProductRepository repository,IMediator mediator)
    {
        _repository = repository;
        _mediator = mediator;
    }

    public async Task<Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product =  await _repository.GetByIdAsync(request.Id);

        if (product == null)
            throw HepsiFlyExceptions.ProductNotFound;

        if (!string.IsNullOrEmpty(product.CategoryId))
            product.Category = await _mediator.Send(new GetCategoryByIdQuery() { Id = product.CategoryId });
        
        return product;
    }
}
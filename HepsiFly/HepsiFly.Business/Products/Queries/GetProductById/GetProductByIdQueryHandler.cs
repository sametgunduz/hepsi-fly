using HepsiFly.Business.Cache;
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
    private readonly ICacheService _cache;

    public GetProductByIdQueryHandler(IProductRepository repository,IMediator mediator,ICacheService cache)
    {
        _repository = repository;
        _mediator = mediator;
        _cache = cache;
    }

    public async Task<Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        if (_cache.IsSet(request.Id))
        {
            return _cache.Get<Product>(request.Id);
        }

        var product =  await _repository.GetByIdAsync(request.Id);

        if (product == null)
            throw HepsiFlyExceptions.ProductNotFound;

        if (!string.IsNullOrEmpty(product.CategoryId))
            product.Category = await _mediator.Send(new GetCategoryByIdQuery() { Id = product.CategoryId }, cancellationToken);
            
        _cache.Set(product.Id,product,5);
        
        return product;
    }
}
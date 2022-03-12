using HepsiFly.Common.Exceptions;
using HepsiFly.Domain.Contracts;
using HepsiFly.Domain.Entities;
using MediatR;

namespace HepsiFly.Business.Products.Queries.GetProductsByFilter;

public class GetProductsByFilterQueryHandler : IRequestHandler<GetProductsByFilterQuery, List<Product>>
{
    private readonly IProductRepository _repository;
    private readonly IMediator _mediator;

    public GetProductsByFilterQueryHandler(IProductRepository repository,IMediator mediator)
    {
        _repository = repository;
        _mediator = mediator;
    }

    public async Task<List<Product>> Handle(GetProductsByFilterQuery request, CancellationToken cancellationToken)
    {
        var products = _repository.Get(c => c.Name.Contains(request.Name)).ToList();

        if (products == null)
            throw HepsiFlyExceptions.ProductNotFound;

        return products;
    }
}
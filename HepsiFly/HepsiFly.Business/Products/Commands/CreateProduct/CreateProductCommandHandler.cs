using HepsiFly.Business.Categories.Queries.GetCategoryById;
using HepsiFly.Domain.Contracts;
using HepsiFly.Domain.Entities;
using MediatR;

namespace HepsiFly.Business.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Product>
{
    private readonly IProductRepository _productRepository;
    //private readonly ICategoryRepository _categoryRepository;
    private readonly IMediator _mediator;

    public CreateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
        //_categoryRepository = _categoryRepository;
    }

    public async Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var result = await _productRepository.AddAsync(new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Currency = request.Currency,
            CategoryId = request.CategoryId
        });

        return result;
    }
}
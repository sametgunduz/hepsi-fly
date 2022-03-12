using HepsiFly.Common.Exceptions;
using HepsiFly.Domain.Contracts;
using MediatR;

namespace HepsiFly.Business.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, string>
{
    private readonly IProductRepository _repository;

    public UpdateProductCommandHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<string> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(request.Id);

        if (product == null)
            throw HepsiFlyExceptions.ProductNotFound;
        
        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.Currency = request.Currency;
        product.CategoryId = request.CategoryId;
        
       var result = await _repository.UpdateAsync(product.Id,
            product);

       return result.Id;
    }
}
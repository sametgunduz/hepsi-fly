using HepsiFly.Common.Exceptions;
using HepsiFly.Domain.Contracts;
using MediatR;

namespace HepsiFly.Business.Products.Commands.DeleteProduct;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, string>
{
    private readonly IProductRepository _repository;

    public DeleteProductCommandHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<string> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(request.Id);
        
        if (product == null)
            throw HepsiFlyExceptions.ProductNotFound;
        
        await _repository.DeleteAsync(product.Id);
        
        return product.Id;
    }
}
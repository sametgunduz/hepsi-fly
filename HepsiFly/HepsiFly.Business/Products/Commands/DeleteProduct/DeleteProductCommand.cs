using MediatR;

namespace HepsiFly.Business.Products.Commands.DeleteProduct;

public class DeleteProductCommand : IRequest<string>
{
    public string Id { get; set; }
}
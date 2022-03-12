using HepsiFly.Domain.Entities;
using MediatR;

namespace HepsiFly.Business.Products.Queries.GetProductById;

public class GetProductByIdQuery : IRequest<Product>
{
    public string Id { get; set; }
}
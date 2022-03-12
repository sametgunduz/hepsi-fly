using HepsiFly.Domain.Entities;
using MediatR;

namespace HepsiFly.Business.Products.Queries.GetProductsByFilter;

public class GetProductsByFilterQuery : IRequest<List<Product>>
{
    public string Name { get; set; }
}
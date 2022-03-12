using HepsiFly.Domain.Entities;
using HepsiFly.Domain.Enums;
using MediatR;

namespace HepsiFly.Business.Products.Commands.CreateProduct;

public class CreateProductCommand : IRequest<Product>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string CategoryId { get; set; }
    public decimal Price { get; set; }
    public Currency Currency { get; set; }
}
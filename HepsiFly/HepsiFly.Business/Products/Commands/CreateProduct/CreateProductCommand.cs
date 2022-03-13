using HepsiFly.Domain.Entities;
using HepsiFly.Domain.Enums;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HepsiFly.Business.Products.Commands.CreateProduct;

public class CreateProductCommand : IRequest<Product>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string CategoryId { get; set; }
    public decimal Price { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    public Currency Currency { get; set; }
}
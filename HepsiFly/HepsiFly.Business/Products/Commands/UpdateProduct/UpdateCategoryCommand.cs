using System.Text.Json.Serialization;
using HepsiFly.Domain.Enums;
using MediatR;

namespace HepsiFly.Business.Products.Commands.UpdateProduct;

public class UpdateProductCommand : IRequest<string>
{
    [JsonIgnore]
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string CategoryId { get; set; }
    public decimal Price { get; set; }
    public Currency Currency { get; set; }
}
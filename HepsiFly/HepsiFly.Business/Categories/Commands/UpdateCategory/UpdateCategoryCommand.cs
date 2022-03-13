using System.Text.Json.Serialization;
using MediatR;

namespace HepsiFly.Business.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommand : IRequest<string>
{
    public string? Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
}
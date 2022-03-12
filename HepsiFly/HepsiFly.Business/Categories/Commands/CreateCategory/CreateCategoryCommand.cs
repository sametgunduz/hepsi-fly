using HepsiFly.Domain.Entities;
using MediatR;

namespace HepsiFly.Business.Categories.Commands.CreateCategory;

public class CreateCategoryCommand : IRequest<Category>
{
    public string Name { get; set; }
    public string Description { get; set; }
}
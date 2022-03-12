using HepsiFly.Domain.Entities;
using MediatR;

namespace HepsiFly.Business.Categories.Queries.GetCategoryById;

public class GetCategoryByIdQuery : IRequest<Category>
{
    public string Id { get; set; }
}
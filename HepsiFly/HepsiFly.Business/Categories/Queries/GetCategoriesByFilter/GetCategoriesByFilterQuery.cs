using HepsiFly.Domain.Entities;
using MediatR;

namespace HepsiFly.Business.Categories.Queries.GetCategoriesByFilter;

public class GetCategoriesByFilterQuery : IRequest<List<Category>>
{
    public string Name { get; set; }
}
using HepsiFly.Domain.Contracts;
using HepsiFly.Domain.Entities;
using MediatR;

namespace HepsiFly.Business.Categories.Queries.GetCategoriesByFilter;

public class GetCategoriesByFilterQueryHandler : IRequestHandler<GetCategoriesByFilterQuery, List<Category>>
{
    private readonly ICategoryRepository _repository;

    public GetCategoriesByFilterQueryHandler(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Category>> Handle(GetCategoriesByFilterQuery request, CancellationToken cancellationToken)
    {
        return _repository.Get(c => c.Name.Contains(request.Name)).ToList();
    }
}
using HepsiFly.Domain.Contracts;
using HepsiFly.Domain.Entities;
using MediatR;

namespace HepsiFly.Business.Categories.Commands.CreateCategory;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Category>
{
    private readonly ICategoryRepository _repository;

    public CreateCategoryCommandHandler(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<Category> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.AddAsync(new Category
        {
            Name = request.Name,
            Description = request.Description
        });

        return result;
    }
}
using HepsiFly.Common.Exceptions;
using HepsiFly.Domain.Contracts;
using MediatR;

namespace HepsiFly.Business.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, string>
{
    private readonly ICategoryRepository _repository;

    public UpdateCategoryCommandHandler(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<string> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _repository.GetByIdAsync(request.Id);

        if (category == null)
            throw HepsiFlyExceptions.CategoryNotFound;
        
        category.Name = request.Name;
        category.Description = request.Description;

       var result = await _repository.UpdateAsync(category.Id,
            category);

       return result.Id;
    }
}
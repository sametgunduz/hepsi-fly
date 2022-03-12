using HepsiFly.Common.Exceptions;
using HepsiFly.Domain.Contracts;
using MediatR;

namespace HepsiFly.Business.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, string>
{
    private readonly ICategoryRepository _repository;

    public DeleteCategoryCommandHandler(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<string> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _repository.GetByIdAsync(request.Id);
        
        if (category == null)
            throw HepsiFlyExceptions.CategoryNotFound;
        
        await _repository.DeleteAsync(category.Id);
        
        return category.Id;
    }
}
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
        var result = await _repository.DeleteAsync(request.Id);
        
        if (result == null)
            throw HepsiFlyExceptions.CategoryNotFound;
        
        return result.Id;
    }
}
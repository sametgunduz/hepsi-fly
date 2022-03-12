using FluentValidation;
using HepsiFly.Business.Categories.Commands.DeleteCategory;

namespace HepsiFly.Business.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommandValidator:  AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .NotNull()
            .WithName("id");
    }
}
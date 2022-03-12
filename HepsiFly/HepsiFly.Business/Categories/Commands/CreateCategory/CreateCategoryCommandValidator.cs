using FluentValidation;

namespace HepsiFly.Business.Categories.Commands.CreateCategory;

public class CreateCategoryCommandValidator:  AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .NotNull()
            .WithName("name");
    }
}
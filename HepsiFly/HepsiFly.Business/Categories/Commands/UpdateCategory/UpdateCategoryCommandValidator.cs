using FluentValidation;

namespace HepsiFly.Business.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandValidator:  AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .NotNull()
            .WithName("id");
        
        RuleFor(x => x.Name)
            .NotEmpty()
            .NotNull()
            .WithName("name");
    }
}
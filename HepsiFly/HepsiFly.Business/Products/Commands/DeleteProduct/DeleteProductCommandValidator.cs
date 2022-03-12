using FluentValidation;
using HepsiFly.Business.Categories.Commands.DeleteCategory;

namespace HepsiFly.Business.Products.Commands.DeleteProduct;

public class DeleteProductCommandValidator:  AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .NotNull()
            .WithName("id");
    }
}
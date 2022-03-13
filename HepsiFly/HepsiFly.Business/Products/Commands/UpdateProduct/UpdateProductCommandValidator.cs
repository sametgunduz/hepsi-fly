using FluentValidation;

namespace HepsiFly.Business.Products.Commands.UpdateProduct;

public class UpdateProductCommandValidator:  AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .NotNull()
            .WithName("name");
        
        RuleFor(x => x.Price)
            .NotEmpty()
            .NotNull()
            .WithName("price");
    }
}
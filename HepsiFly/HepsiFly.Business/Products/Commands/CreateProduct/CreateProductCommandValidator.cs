using FluentValidation;

namespace HepsiFly.Business.Products.Commands.CreateProduct;

public class CreateProductCommandValidator:  AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .NotNull()
            .WithName("name");
        
        RuleFor(x => x.Price)
            .NotEmpty()
            .NotNull()
            .WithName("price");
        
        RuleFor(x => x.Currency)
            .NotEmpty()
            .NotNull()
            .WithName("currency");
    }
}
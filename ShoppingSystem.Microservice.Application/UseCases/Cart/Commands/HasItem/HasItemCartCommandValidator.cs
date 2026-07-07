using FluentValidation;
using ShoppingSystem.Microservice.Resource.Messages;

namespace ShoppingSystem.Microservice.Application.UseCases.Cart.Commands.HasItem;

public sealed class HasItemCartCommandValidator
    : AbstractValidator<HasItemCartCommand>
{
    public HasItemCartCommandValidator()
    {
        RuleFor(x => x.CartId)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.CartId)));

        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.ProductId)));
    }
}
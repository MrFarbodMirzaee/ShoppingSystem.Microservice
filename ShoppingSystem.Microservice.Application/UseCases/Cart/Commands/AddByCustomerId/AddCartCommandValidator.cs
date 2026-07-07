using FluentValidation;
using ShoppingSystem.Microservice.Resource.Messages;

namespace ShoppingSystem.Microservice.Application.UseCases.Cart.Commands.AddByCustomerId;

public sealed class AddCartCommandValidator
    : AbstractValidator<AddCartCommand>
{
    public AddCartCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.CustomerId)));
    }
}
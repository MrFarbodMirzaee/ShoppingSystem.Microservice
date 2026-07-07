using FluentValidation;
using ShoppingSystem.Microservice.Resource.Messages;

namespace ShoppingSystem.Microservice.Application.UseCases.Order.Commands.Add;

public sealed class AddOrderCommandValidator
    : AbstractValidator<AddOrderCommand>
{
    public AddOrderCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.UserId)));

        RuleFor(x => x.OrderNumber)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.OrderNumber)))
            .MaximumLength(50)
            .WithMessage(prop => string.Format(
                Errors.Maximum,
                nameof(prop.OrderNumber),
                50));
    }
}
using FluentValidation;
using ShoppingSystem.Microservice.Resource.Messages;

namespace ShoppingSystem.Microservice.Application.UseCases.Payment.Commands.Add;

public sealed class AddPaymentCommandValidator
    : AbstractValidator<AddPaymentCommand>
{
    public AddPaymentCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.OrderId)));

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage(prop => string.Format(
                Errors.GreaterThan,
                nameof(prop.Amount),
                0));

        RuleFor(x => x.Currency)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.Currency)))
            .MaximumLength(3)
            .WithMessage(prop => string.Format(
                Errors.Maximum,
                nameof(prop.Currency),
                3));
    }
}
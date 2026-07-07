using FluentValidation;
using ShoppingSystem.Microservice.Resource.Messages;

namespace ShoppingSystem.Microservice.Application.UseCases.Order.Commands.Update;

public sealed class UpdateOrderCommandValidator
    : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.OrderId)));

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage(prop => string.Format(
                Errors.Invalid,
                nameof(prop.Status)));
    }
}
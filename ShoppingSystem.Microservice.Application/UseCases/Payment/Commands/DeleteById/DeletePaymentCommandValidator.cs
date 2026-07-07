using FluentValidation;
using ShoppingSystem.Microservice.Resource.Messages;

namespace ShoppingSystem.Microservice.Application.UseCases.Payment.Commands.DeleteById;

public sealed class DeleteByIdPaymentCommandValidator
    : AbstractValidator<DeleteByIdPaymentCommand>
{
    public DeleteByIdPaymentCommandValidator()
    {
        RuleFor(x => x.PaymentId)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.PaymentId)));
    }
}
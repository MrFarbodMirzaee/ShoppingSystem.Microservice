using FluentValidation;
using ShoppingSystem.Microservice.Resource.Messages;

namespace ShoppingSystem.Microservice.Application.UseCases.Payment.Queries.GetById;

public sealed class GetByIdPaymentQueryValidator
    : AbstractValidator<GetByIdPaymentQuery>
{
    public GetByIdPaymentQueryValidator()
    {
        RuleFor(x => x.PaymentId)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.PaymentId)));
    }
}
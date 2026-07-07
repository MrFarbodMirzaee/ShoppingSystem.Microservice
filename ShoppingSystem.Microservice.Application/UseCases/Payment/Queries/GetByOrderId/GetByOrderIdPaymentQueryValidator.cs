using FluentValidation;
using ShoppingSystem.Microservice.Resource.Messages;

namespace ShoppingSystem.Microservice.Application.UseCases.Payment.Queries.GetByOrderId;

public sealed class GetByOrderIdPaymentQueryValidator
    : AbstractValidator<GetByOrderIdPaymentQuery>
{
    public GetByOrderIdPaymentQueryValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.OrderId)));
    }
}
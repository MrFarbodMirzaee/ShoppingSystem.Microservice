using FluentValidation;
using ShoppingSystem.Microservice.Resource.Messages;

namespace ShoppingSystem.Microservice.Application.UseCases.Payment.Queries.GetByStatus;

public sealed class GetByStatusPaymentQueryValidator
    : AbstractValidator<GetByStatusPaymentQuery>
{
    public GetByStatusPaymentQueryValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage(prop => string.Format(
                Errors.Invalid,
                nameof(prop.Status)));
    }
}
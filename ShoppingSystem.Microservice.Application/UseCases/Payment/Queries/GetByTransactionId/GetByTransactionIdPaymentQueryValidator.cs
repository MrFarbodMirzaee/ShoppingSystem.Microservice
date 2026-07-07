using FluentValidation;
using ShoppingSystem.Microservice.Resource.Messages;

namespace ShoppingSystem.Microservice.Application.UseCases.Payment.Queries.GetByTransactionId;

public sealed class GetByTransactionIdPaymentQueryValidator
    : AbstractValidator<GetByTransactionIdPaymentQuery>
{
    public GetByTransactionIdPaymentQueryValidator()
    {
        RuleFor(x => x.TransactionId)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.TransactionId)));
    }
}
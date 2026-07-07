using FluentValidation;
using ShoppingSystem.Microservice.Resource.Messages;

namespace ShoppingSystem.Microservice.Application.UseCases.Order.Queries.GetByStatus;

public sealed class GetByStatusOrderQueryValidator
    : AbstractValidator<GetByStatusOrderQuery>
{
    public GetByStatusOrderQueryValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage(prop => string.Format(
                Errors.Invalid,
                nameof(prop.Status)));
    }
}
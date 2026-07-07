using FluentValidation;
using ShoppingSystem.Microservice.Resource.Messages;

namespace ShoppingSystem.Microservice.Application.UseCases.Order.Queries.GetByNumber;

public sealed class GetByNumberOrderQueryValidator
    : AbstractValidator<GetByNumberOrderQuery>
{
    public GetByNumberOrderQueryValidator()
    {
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
using FluentValidation;
using ShoppingSystem.Microservice.Resource.Messages;

namespace ShoppingSystem.Microservice.Application.UseCases.Order.Queries.GetById;

public sealed class GetByIdOrderQueryValidator
    : AbstractValidator<GetByIdOrderQuery>
{
    public GetByIdOrderQueryValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.OrderId)));
    }
}
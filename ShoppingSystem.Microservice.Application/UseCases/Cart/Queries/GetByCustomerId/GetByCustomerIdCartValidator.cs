using FluentValidation;
using ShoppingSystem.Microservice.Resource.Messages;

namespace ShoppingSystem.Microservice.Application.UseCases.Cart.Queries.GetByCustomerId;

public sealed class GetByCustomerIdCartQueryValidator
    : AbstractValidator<GetByCustomerIdCartQuery>
{
    public GetByCustomerIdCartQueryValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.CustomerId)));
    }
}
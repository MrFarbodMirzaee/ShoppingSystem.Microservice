using FluentValidation;
using ShoppingSystem.Microservice.Resource.Messages;

namespace ShoppingSystem.Microservice.Application.UseCases.Address.Queries.GetByState;

public sealed class GetByStateAddressQueryValidator
    : AbstractValidator<GetByStateAddressQuery>
{
    public GetByStateAddressQueryValidator()
    {
        RuleFor(x => x.State)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.State)))
            .MaximumLength(100)
            .WithMessage(prop => string.Format(
                Errors.Maximum,
                nameof(prop.State),
                100));
    }
}
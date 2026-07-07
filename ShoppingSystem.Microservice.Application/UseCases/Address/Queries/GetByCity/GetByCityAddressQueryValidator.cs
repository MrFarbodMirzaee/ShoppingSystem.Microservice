using FluentValidation;
using ShoppingSystem.Microservice.Resource.Messages;

namespace ShoppingSystem.Microservice.Application.UseCases.Address.Queries.GetByCity;

public class GetByCityAddressQueryValidator
    : AbstractValidator<GetByCityAddressQuery>
{
    public GetByCityAddressQueryValidator()
    {
        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.City)))
            .MaximumLength(50)
            .WithMessage(prop => string.Format(
                Errors.Maximum,
                nameof(prop.City),
                50))
            .MinimumLength(2)
            .WithMessage(prop => string.Format(
                Errors.Minimum,
                nameof(prop.City),
                2));
    }
}
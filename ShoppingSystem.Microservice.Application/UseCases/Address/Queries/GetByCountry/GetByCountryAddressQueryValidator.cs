using FluentValidation;
using ShoppingSystem.Microservice.Resource.Messages;

namespace ShoppingSystem.Microservice.Application.UseCases.Address.Queries.GetByCountry;

public sealed class GetByCountryAddressQueryValidator
    : AbstractValidator<GetByCountryAddressQuery>
{
    public GetByCountryAddressQueryValidator()
    {
        RuleFor(x => x.Country)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.Country)))
            .MaximumLength(100)
            .WithMessage(prop => string.Format(
                Errors.Maximum,
                nameof(prop.Country),
                100));
    }
}
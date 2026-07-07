using FluentValidation;
using ShoppingSystem.Microservice.Resource.Messages;

namespace ShoppingSystem.Microservice.Application.UseCases.Address.Queries.GetByPostalCode;

public sealed class GetByPostalCodeAddressQueryValidator
    : AbstractValidator<GetByPostalCodeAddressQuery>
{
    public GetByPostalCodeAddressQueryValidator()
    {
        RuleFor(x => x.PostalCode)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.PostalCode)));
    }
}
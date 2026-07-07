using FluentValidation;
using ShoppingSystem.Microservice.Resource.Messages;

namespace ShoppingSystem.Microservice.Application.UseCases.Address.Queries.GetById;

public sealed class GetByIdAddressQueryValidator
    : AbstractValidator<GetByIdAddressQuery>
{
    public GetByIdAddressQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.Id)));
    }
}
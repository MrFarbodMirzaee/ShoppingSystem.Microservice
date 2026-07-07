using FluentValidation;
using ShoppingSystem.Microservice.Resource.Messages;

namespace ShoppingSystem.Microservice.Application.UseCases.Address.Commands.DeleteById;

public class DeleteByIdAddressCommandRequestValidator
    : AbstractValidator<DeleteByIdAddressCommandRequest>
{
    public DeleteByIdAddressCommandRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.Id)));

        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty)
            .WithMessage(prop => string.Format(
                Errors.Invalid,
                nameof(prop.Id)));
    }
}
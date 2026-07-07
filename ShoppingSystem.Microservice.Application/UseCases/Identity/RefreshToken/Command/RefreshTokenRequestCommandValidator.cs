using FluentValidation;
using ShoppingSystem.Microservice.Resource.Messages;

namespace ShoppingSystem.Microservice.Application.UseCases.Identity.RefreshToken.Command;

public class RefreshTokenRequestCommandValidator
    : AbstractValidator<RefreshTokenRequestCommand>
{
    public RefreshTokenRequestCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.RefreshToken)))
            .MinimumLength(20)
            .WithMessage(prop => string.Format(
                Errors.Minimum,
                nameof(prop.RefreshToken),
                20))
            .MaximumLength(512)
            .WithMessage(prop => string.Format(
                Errors.Maximum,
                nameof(prop.RefreshToken),
                512));
    }
}
using FluentValidation;
using ShoppingSystem.Microservice.Resource.Messages;

namespace ShoppingSystem.Microservice.Application.UseCases.Identity.SignIn.Commands;

public class SignInCommandValidator 
    : AbstractValidator<SignInCommandRequest>
{
    public SignInCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.Email)))
            .EmailAddress()
            .WithMessage(prop => string.Format(
                Errors.InvalidFormat,
                nameof(prop.Email)))
            .MaximumLength(256)
            .WithMessage(prop => string.Format(
                Errors.Maximum,
                nameof(prop.Email),
                256));

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.Password)))
            .MinimumLength(8)
            .WithMessage(prop => string.Format(
                Errors.Minimum,
                nameof(prop.Password),
                8))
            .MaximumLength(100)
            .WithMessage(prop => string.Format(
                Errors.Maximum,
                nameof(prop.Password),
                100));
    }
}
using FluentValidation;
using ShoppingSystem.Microservice.Resource.Messages;

namespace ShoppingSystem.Microservice.Application.UseCases.Identity.SignUp.Commands;

public class SignUpCommandValidator 
    : AbstractValidator<SignUpCommandRequest>
{
    public SignUpCommandValidator()
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
                100))
            .Matches("[A-Z]")
            .WithMessage(prop => string.Format(
                Errors.Invalid,
                nameof(prop.Password)))
            .Matches("[a-z]")
            .WithMessage(prop => string.Format(
                Errors.Invalid,
                nameof(prop.Password)))
            .Matches("[0-9]")
            .WithMessage(prop => string.Format(
                Errors.Invalid,
                nameof(prop.Password)))
            .Matches("[^a-zA-Z0-9]")
            .WithMessage(prop => string.Format(
                Errors.Invalid,
                nameof(prop.Password)));

        RuleFor(x => x.FirstName)
            .MaximumLength(100)
            .WithMessage(prop => string.Format(
                Errors.Maximum,
                nameof(prop.FirstName),
                100));

        RuleFor(x => x.LastName)
            .MaximumLength(100)
            .WithMessage(prop => string.Format(
                Errors.Maximum,
                nameof(prop.LastName),
                100));
    }
}
using FluentValidation;
using ShoppingSystem.Microservice.Resource.Messages;

namespace ShoppingSystem.Microservice.Application.UseCases.Identity.GoogleSignIn.Commands;

public class GoogleSignInCommandValidator
    : AbstractValidator<GoogleSignInCommandRequest>
{
    public GoogleSignInCommandValidator()
    {
        RuleFor(x => x.IdToken)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.IdToken)));
    }
}
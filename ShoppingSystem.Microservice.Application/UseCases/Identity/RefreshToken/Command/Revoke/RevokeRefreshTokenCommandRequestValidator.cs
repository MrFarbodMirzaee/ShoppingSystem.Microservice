using FluentValidation;

namespace ShoppingSystem.Microservice.Application.UseCases.Identity.RefreshToken.Command.Revoke;

public class RevokeRefreshTokenCommandRequestValidator
    : AbstractValidator<RevokeRefreshTokenCommandRequest>
{
    public RevokeRefreshTokenCommandRequestValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage("Refresh token is required.")
            .MinimumLength(20)
            .WithMessage("Refresh token is invalid.")
            .MaximumLength(512)
            .WithMessage("Refresh token is too long.");
    }
}
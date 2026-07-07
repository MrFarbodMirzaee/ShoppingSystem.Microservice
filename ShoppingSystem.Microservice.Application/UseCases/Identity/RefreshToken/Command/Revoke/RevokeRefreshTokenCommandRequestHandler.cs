using FluentValidation;
using MediatR;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Identity.RefreshToken.Command.Revoke;

public class RevokeRefreshTokenCommandRequestHandler
    : IRequestHandler<RevokeRefreshTokenCommandRequest, Response<bool>>
{
    private readonly IAuthService _authService;
    private readonly IValidator<RevokeRefreshTokenCommandRequest> _validator;

    public RevokeRefreshTokenCommandRequestHandler
        (
            IAuthService authService,
            IValidator<RevokeRefreshTokenCommandRequest> validator
        )
        {
            _authService = authService;
            _validator = validator;
        }

    public async Task<Response<bool>> Handle(
        RevokeRefreshTokenCommandRequest request,
        CancellationToken ct)
    {
        
        var validationResult = await _validator
            .ValidateAsync(request, ct);

        if (!validationResult.IsValid)
        {
            return new Response<bool>
            {
                Succeeded = false,
                Message = "Validation failed.",
                Errors = validationResult.Errors
                    .Select(x => x.ErrorMessage)
                    .ToList(),
                Data = false
            };
        }
        
        return await _authService.RevokeRefreshTokenAsync(
            request.RefreshToken,
            ct);
    }
}
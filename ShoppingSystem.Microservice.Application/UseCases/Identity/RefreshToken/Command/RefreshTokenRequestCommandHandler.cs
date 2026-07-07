using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Identity.RefreshToken.Command;

public class RefreshTokenRequestCommandHandler
    : IRequestHandler<RefreshTokenRequestCommand, Response<AuthResponse>>
{
    private readonly IAuthService _authService;

    private readonly ILogger<RefreshTokenRequestCommandHandler> _logger;

    public RefreshTokenRequestCommandHandler(
        IAuthService authService,
        ILogger<RefreshTokenRequestCommandHandler> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    public async Task<Response<AuthResponse>> Handle(
        RefreshTokenRequestCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Starting refresh token process.");

        var result = await _authService.RefreshTokenAsync(
            request.RefreshToken,
            cancellationToken);

        if (!result.Succeeded)
        {
            _logger.LogWarning(
                "Refresh token failed. Reason: {Reason}",
                result.Message);

            return result;
        }

        _logger.LogInformation(
            "Refresh token generated successfully. UserEmail: {Email}.",
            result.Data?.Email);

        return result;
    }
}
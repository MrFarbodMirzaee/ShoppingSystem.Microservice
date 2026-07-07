using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Events.User;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Identity.GoogleSignIn.Commands;

public class GoogleSignInCommandHandler
    : IRequestHandler<GoogleSignInCommandRequest, Response<AuthResponse>>
{
    private readonly IAuthService _authService;
    private readonly IValidator<GoogleSignInCommandRequest> _validator;
    private readonly IMediator _mediator;
    private readonly ILogger<GoogleSignInCommandHandler> _logger;

    public GoogleSignInCommandHandler(
        IAuthService authService,
        IValidator<GoogleSignInCommandRequest> validator,
        IMediator mediator,
        ILogger<GoogleSignInCommandHandler> logger)
    {
        _authService = authService;
        _validator = validator;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<Response<AuthResponse>> Handle(
        GoogleSignInCommandRequest request,
        CancellationToken ct)
    {
        _logger.LogInformation(
            "Starting Google sign-in process.");

        var validationResult = await _validator
            .ValidateAsync(request, ct);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning(
                "Google sign-in validation failed. Errors: {Errors}",
                validationResult.Errors.Select(x => x.ErrorMessage));

            return new Response<AuthResponse>
            {
                Succeeded = false,
                Message = "Validation failed.",
                Errors = validationResult.Errors
                    .Select(x => x.ErrorMessage)
                    .ToList()
            };
        }

        var result = await _authService.GoogleLoginAsync(
            request.IdToken,
            ct);

        if (!result.Succeeded)
        {
            _logger.LogWarning(
                "Google sign-in failed. Reason: {Reason}",
                result.Message);

            return new Response<AuthResponse>(result.Message);
        }

        if (result.Data!.IsNewUser)
        {
            _logger.LogInformation(
                "New user registered via Google. Email: {Email}.",
                result.Data.Email);

            await _mediator.Publish(
                new UserRegisteredEvent(
                    result.Data.Email,
                    result.Data.FirstName),
                ct);
        }
        else
        {
            _logger.LogInformation(
                "Existing user signed in via Google. Email: {Email}.",
                result.Data.Email);

            await _mediator.Publish(
                new UserSignedInEvent(
                    result.Data.Email,
                    result.Data.FirstName),
                ct);
        }

        _logger.LogInformation(
            "Google sign-in completed successfully for {Email}.",
            result.Data.Email);

        return new Response<AuthResponse>(result.Data);
    }
}
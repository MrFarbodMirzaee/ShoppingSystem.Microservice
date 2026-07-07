using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Events.User;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Identity.SignIn.Commands;

public class SignInCommandHandler 
    : IRequestHandler<SignInCommandRequest, Response<AuthResponse>>
{
    private readonly IAuthService _authService;
    private readonly IValidator<SignInCommandRequest> _validator;
    private readonly IMediator _mediator;
    private readonly ILogger<SignInCommandHandler> _logger;

    public SignInCommandHandler(IAuthService authService, IValidator<SignInCommandRequest> validator, IMediator mediator, ILogger<SignInCommandHandler> logger)
    {
        _authService = authService;
        _validator = validator;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<Response<AuthResponse>> Handle(
    SignInCommandRequest request,
    CancellationToken ct)
{
    _logger.LogInformation(
        "Starting user sign-in. Email: {Email}.",
        request.Email);

    var validationResult = await _validator
        .ValidateAsync(request, ct);

    if (!validationResult.IsValid)
    {
        _logger.LogWarning(
            "Sign-in validation failed for Email: {Email}. Errors: {Errors}",
            request.Email,
            validationResult.Errors.Select(x => x.ErrorMessage));

        return new Response<AuthResponse>
        {
            Succeeded = false,
            Message = "Validation failed.",
            Errors = validationResult.Errors
                .Select(x => x.ErrorMessage)
                .ToList(),
            Data = null
        };
    }

    var result = await _authService.SignInAsync(
        request.Email,
        request.Password,
        ct);

    if (!result.Succeeded)
    {
        _logger.LogWarning(
            "Sign-in failed for Email: {Email}. Reason: {Reason}",
            request.Email,
            result.Message);

        return new Response<AuthResponse>(result.Message);
    }

    await _mediator.Publish(
        new UserSignedInEvent(
            result.Data!.Email,
            result.Data.FirstName),
        ct);

    _logger.LogInformation(
        "User signed in successfully. Email: {Email}.",
        result.Data.Email);

    return new Response<AuthResponse>(result.Data);
}
}
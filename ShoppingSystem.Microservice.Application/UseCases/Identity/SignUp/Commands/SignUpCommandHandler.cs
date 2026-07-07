using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Events.User;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Identity.SignUp.Commands;

public class SignUpCommandHandler
    : IRequestHandler<SignUpCommandRequest, Response<AuthResponse>>
{
    private readonly IAuthService _authService;
    private readonly IValidator<SignUpCommandRequest> _validator;
    private readonly IMediator _mediator;
    private readonly ILogger<SignUpCommandHandler> _logger;

    public SignUpCommandHandler(IAuthService authService, IValidator<SignUpCommandRequest> validator, IMediator mediator, ILogger<SignUpCommandHandler> logger)
    {
        _authService = authService;
        _validator = validator;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<Response<AuthResponse>> Handle(
        SignUpCommandRequest request,
        CancellationToken ct)
    {
        _logger.LogInformation(
            "Starting user registration. Email: {Email}.",
            request.Email);

        var validationResult = await _validator
            .ValidateAsync(request, ct);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning(
                "User registration validation failed for Email: {Email}. Errors: {Errors}",
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

        var result = await _authService.SignUpAsync(
            request.Email,
            request.Password,
            request.FirstName,
            request.LastName,
            ct);

        if (!result.Succeeded)
        {
            _logger.LogWarning(
                "User registration failed for Email: {Email}. Reason: {Reason}",
                request.Email,
                result.Message);

            return new Response<AuthResponse>(result.Message);
        }

        await _mediator.Publish(
            new UserRegisteredEvent(
                request.Email,
                request.FirstName),
            ct);

        _logger.LogInformation(
            "User registered successfully. Email: {Email}.",
            request.Email);

        return new Response<AuthResponse>(result.Data);
    }
}
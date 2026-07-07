using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Cart.Commands.AddByCustomerId;

public sealed class AddCartCommandHandler
    : IRequestHandler<AddCartCommand, Response<bool>>
{
    private readonly ICartService _cartService;
    private readonly IUnitOfWorkBase _unitOfWork;
    private readonly IValidator<AddCartCommand> _validator;
    private readonly IUserQueryService _userQueryService;
    private readonly ILogger<AddCartCommandHandler> _logger;
    
    public AddCartCommandHandler
    (
        ICartService cartService,
        IUnitOfWorkBase unitOfWork,
        IValidator<AddCartCommand> validator,
        IUserQueryService userQueryService,
        ILogger<AddCartCommandHandler> logger
    )
    {
        _cartService = cartService;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _userQueryService  = userQueryService;
        _logger = logger;
    }

    public async Task<Response<bool>> Handle(
        AddCartCommand request,
        CancellationToken ct)
    {
        _logger.LogInformation(
            "Starting cart creation for CustomerId {CustomerId}.",
            request.CustomerId);

        var validationResult = await _validator
            .ValidateAsync(request, ct);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning(
                "Cart creation validation failed for CustomerId {CustomerId}. Errors: {Errors}",
                request.CustomerId,
                validationResult.Errors.Select(x => x.ErrorMessage));

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

        var userExists = await _userQueryService
            .UserExistsAsync(request.UserId, ct);

        if (!userExists)
        {
            _logger.LogWarning(
                "Cart creation failed. User {UserId} does not exist.",
                request.UserId);

            return new Response<bool>(
                false,
                "User does not exist.");
        }

        var exists = await _cartService
            .ExistsByCustomerIdAsync(request.CustomerId, ct);

        if (exists.Data)
        {
            _logger.LogWarning(
                "Cart creation failed. Customer {CustomerId} already has a cart.",
                request.CustomerId);

            return new Response<bool>(
                false,
                "Customer already has a cart.");
        }

        var cart = new Domain.Aggregates.Cart.Cart(
            request.CustomerId);

        _cartService.Add(cart, ct);

        await _unitOfWork.SaveAsync(ct);

        _logger.LogInformation(
            "Cart created successfully for CustomerId {CustomerId}. CartId: {CartId}.",
            request.CustomerId,
            cart.Id);

        return new Response<bool>(true);
    }
}
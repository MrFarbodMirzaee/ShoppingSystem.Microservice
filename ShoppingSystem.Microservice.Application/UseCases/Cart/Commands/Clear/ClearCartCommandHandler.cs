using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Cart.Commands.Clear;

public class ClearCartCommandHandler
    : IRequestHandler<ClearCartCommand, Response<bool>>
{
    private readonly ICartService _cartService;
    private readonly IUnitOfWorkBase _unitOfWork;
    private readonly IValidator<ClearCartCommand> _validator;
    private readonly ILogger<ClearCartCommandHandler> _logger;

    public ClearCartCommandHandler
    (
        ICartService cartService,
        IUnitOfWorkBase unitOfWork,
        IValidator<ClearCartCommand> validator,
        ILogger<ClearCartCommandHandler> logger
    )
    {
        _cartService = cartService;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Response<bool>> Handle(
        ClearCartCommand request,
        CancellationToken ct)
    {
        _logger.LogInformation(
            "Starting cart clear operation. CartId: {CartId}.",
            request.CartId);

        var validationResult = await _validator
            .ValidateAsync(request, ct);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning(
                "Cart clear validation failed for CartId {CartId}. Errors: {Errors}",
                request.CartId,
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

        var cartResponse = await _cartService
            .GetByIdAsync(request.CartId, ct);

        if (!cartResponse.Succeeded || cartResponse.Data is null)
        {
            _logger.LogWarning(
                "Cart clear failed. CartId {CartId} was not found.",
                request.CartId);

            return new Response<bool>("Cart not found.");
        }

        var cart = cartResponse.Data;

        cart.Clear();

        await _cartService.UpdateAsync(cart, ct);

        await _unitOfWork.SaveAsync(ct);

        _logger.LogInformation(
            "Cart cleared successfully. CartId: {CartId}.",
            request.CartId);

        return new Response<bool>(true);
    }
}
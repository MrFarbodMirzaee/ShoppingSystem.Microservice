using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Cart.Commands.Update;

public class UpdateCartCommandHandler
    : IRequestHandler<UpdateCartCommand, Response<bool>>
{
    private readonly ICartService _cartService;
    private readonly IUnitOfWorkBase _unitOfWork;
    private readonly IValidator<UpdateCartCommand> _validator;
     private readonly ILogger<UpdateCartCommandHandler> _logger;

    public UpdateCartCommandHandler
    (
        ICartService cartService,
        IUnitOfWorkBase unitOfWork,
        IValidator<UpdateCartCommand> validator,
        ILogger<UpdateCartCommandHandler> logger
    )
    {
        _cartService = cartService;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Response<bool>> Handle(
    UpdateCartCommand request,
    CancellationToken ct)
{
    _logger.LogInformation(
        "Starting cart update. CustomerId: {CustomerId}, ProductId: {ProductId}.",
        request.CustomerId,
        request.ProductId);

    var validationResult = await _validator
        .ValidateAsync(request, ct);

    if (!validationResult.IsValid)
    {
        _logger.LogWarning(
            "Cart update validation failed for CustomerId {CustomerId}. Errors: {Errors}",
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

    var cartResponse = await _cartService
        .GetByCustomerIdAsync(request.CustomerId, ct);

    if (!cartResponse.Succeeded || cartResponse.Data is null)
    {
        _logger.LogWarning(
            "Cart update failed. Cart for CustomerId {CustomerId} was not found.",
            request.CustomerId);

        return new Response<bool>(
            false,
            "Cart not found.");
    }

    var cart = cartResponse.Data;

    var item = cart.Items
        .FirstOrDefault(x => x.ProductId == request.ProductId);

    if (item is null)
    {
        _logger.LogWarning(
            "Cart update failed. ProductId {ProductId} does not exist in CustomerId {CustomerId}'s cart.",
            request.ProductId,
            request.CustomerId);

        return new Response<bool>(
            false,
            "Product does not exist in cart.");
    }

    item.UpdateQuantity(request.Quantity);

    await _cartService.UpdateAsync(cart, ct);

    await _unitOfWork.SaveAsync(ct);

    _logger.LogInformation(
        "Cart updated successfully. CustomerId: {CustomerId}, ProductId: {ProductId}, NewQuantity: {Quantity}.",
        request.CustomerId,
        request.ProductId,
        request.Quantity);

    return new Response<bool>(true);
}
}
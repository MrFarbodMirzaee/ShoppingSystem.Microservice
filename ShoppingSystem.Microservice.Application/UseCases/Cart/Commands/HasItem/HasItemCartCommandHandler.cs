using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Cart.Commands.HasItem;

public class HasItemCartCommandHandler
    : IRequestHandler<HasItemCartCommand, Response<bool>>
{
    private readonly ICartService _cartService;
    private readonly IValidator<HasItemCartCommand> _validator;
    private readonly ILogger<HasItemCartCommandHandler> _logger;

    public HasItemCartCommandHandler
    (
        ICartService cartService,
        IValidator<HasItemCartCommand> validator,
        ILogger<HasItemCartCommandHandler> logger
    )
    {
        _cartService = cartService;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Response<bool>> Handle(
        HasItemCartCommand request,
        CancellationToken ct)
    {
        _logger.LogInformation(
            "Checking whether CartId {CartId} contains ProductId {ProductId}.",
            request.CartId,
            request.ProductId);

        var validationResult = await _validator
            .ValidateAsync(request, ct);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning(
                "HasItemCart validation failed. CartId: {CartId}, ProductId: {ProductId}. Errors: {Errors}",
                request.CartId,
                request.ProductId,
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
                "Cart lookup failed. CartId {CartId} was not found.",
                request.CartId);

            return new Response<bool>("Cart not found.");
        }

        var hasItem = cartResponse.Data.Items
            .Any(x => x.ProductId == request.ProductId);

        _logger.LogInformation(
            "Cart item check completed. CartId: {CartId}, ProductId: {ProductId}, Exists: {HasItem}.",
            request.CartId,
            request.ProductId,
            hasItem);

        return new Response<bool>(hasItem);
    }
}
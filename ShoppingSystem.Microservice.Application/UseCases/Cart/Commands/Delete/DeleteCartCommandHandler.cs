using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Cart.Commands.Delete;

public class DeleteCartCommandHandler
    : IRequestHandler<DeleteCartCommand, Response<bool>>
{
    private readonly ICartService _cartService;
    private readonly IUnitOfWorkBase _unitOfWork;
    private readonly IValidator<DeleteCartCommand> _validator;
    private readonly ILogger<DeleteCartCommandHandler> _logger;

    public DeleteCartCommandHandler
    (
        ICartService cartService,
        IUnitOfWorkBase unitOfWork,
        IValidator<DeleteCartCommand> validator,
        ILogger<DeleteCartCommandHandler> logger
    )
    {
        _cartService = cartService;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Response<bool>> Handle(
        DeleteCartCommand request,
        CancellationToken ct)
    {
        _logger.LogInformation(
            "Starting cart deletion. CartId: {CartId}.",
            request.CartId);

        var validationResult = await _validator
            .ValidateAsync(request, ct);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning(
                "Cart deletion validation failed for CartId {CartId}. Errors: {Errors}",
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
                "Cart deletion failed. CartId {CartId} was not found.",
                request.CartId);

            return new Response<bool>(
                false,
                "Cart not found.");
        }

        var cart = cartResponse.Data;

        await _cartService.DeleteByIdAsync(cart.Id, ct);

        await _unitOfWork.SaveAsync(ct);

        _logger.LogInformation(
            "Cart deleted successfully. CartId: {CartId}.",
            request.CartId);

        return new Response<bool>(true);
    }
}
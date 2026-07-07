using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.ValueObjects;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Inventory.Commands.Add;

public class AddInventoryCommandHandler
    : IRequestHandler<AddInventoryCommand, Response<bool>>
{
    private readonly IInventoryService _inventoryService;
    private readonly IUnitOfWorkBase _unitOfWork;
    private readonly IValidator<AddInventoryCommand> _validator;
    private readonly ILogger<AddInventoryCommandHandler> _logger;

    public AddInventoryCommandHandler
    (
        IInventoryService inventoryService,
        IUnitOfWorkBase unitOfWork,
        IValidator<AddInventoryCommand> validator,
        ILogger<AddInventoryCommandHandler> logger
    )
    {
        _inventoryService = inventoryService;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }


    public async Task<Response<bool>> Handle(
        AddInventoryCommand request,
        CancellationToken ct)
    {
        _logger.LogInformation(
            "Starting inventory creation for ProductId: {ProductId}.",
            request.ProductId);

        var validationResult = await _validator
            .ValidateAsync(request, ct);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning(
                "Inventory creation validation failed for ProductId {ProductId}. Errors: {Errors}",
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

        var exists = await _inventoryService
            .ExistsByProductIdAsync(request.ProductId, ct);

        if (exists.Data)
        {
            _logger.LogWarning(
                "Inventory creation failed. Inventory already exists for ProductId {ProductId}.",
                request.ProductId);

            return new Response<bool>(
                false,
                "Inventory already exists for this product.");
        }

        var quantity = StockQuantity.Create(
            request.Quantity);

        var inventory = new Domain.Aggregates.Inventory.Inventory(
            request.ProductId,
            quantity);

        _inventoryService.Add(
            inventory,
            ct);

        await _unitOfWork.SaveAsync(ct);

        _logger.LogInformation(
            "Inventory created successfully. InventoryId: {InventoryId}, ProductId: {ProductId}, Quantity: {Quantity}.",
            inventory.Id,
            inventory.ProductId,
            inventory.Quantity.Value);

        return new Response<bool>(true);
    }
}
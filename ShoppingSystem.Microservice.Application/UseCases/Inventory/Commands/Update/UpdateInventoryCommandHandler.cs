using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Inventory.Commands.Update;

public class UpdateInventoryCommandHandler
    : IRequestHandler<UpdateInventoryCommand, Response<bool>>
{
    private readonly IInventoryService _inventoryService;
    private readonly IUnitOfWorkBase _unitOfWork;
    private readonly IValidator<UpdateInventoryCommand> _validator;
    private readonly ILogger<UpdateInventoryCommandHandler> _logger;
    
    public UpdateInventoryCommandHandler
    (
        IInventoryService inventoryService,
        IUnitOfWorkBase unitOfWork,
        IValidator<UpdateInventoryCommand> validator,
        ILogger<UpdateInventoryCommandHandler> logger
    )
    {
        _inventoryService = inventoryService;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }


    public async Task<Response<bool>> Handle(
    UpdateInventoryCommand request,
    CancellationToken ct)
{
    _logger.LogInformation(
        "Starting inventory update. InventoryId: {InventoryId}, Operation: {Operation}, Quantity: {Quantity}.",
        request.InventoryId,
        request.Increase ? "Increase" : "Decrease",
        request.Quantity);

    var validationResult = await _validator
        .ValidateAsync(request, ct);

    if (!validationResult.IsValid)
    {
        _logger.LogWarning(
            "Inventory update validation failed for InventoryId {InventoryId}. Errors: {Errors}",
            request.InventoryId,
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

    var inventoryResponse = await _inventoryService
        .GetByIdAsync(request.InventoryId, ct);

    if (!inventoryResponse.Succeeded || inventoryResponse.Data is null)
    {
        _logger.LogWarning(
            "Inventory update failed. InventoryId {InventoryId} was not found.",
            request.InventoryId);

        return new Response<bool>(
            false,
            "Inventory not found.");
    }

    var inventory = inventoryResponse.Data;

    if (request.Increase)
    {
        inventory.IncreaseStock(request.Quantity);
    }
    else
    {
        inventory.DecreaseStock(request.Quantity);
    }

    await _inventoryService.UpdateAsync(inventory, ct);

    await _unitOfWork.SaveAsync(ct);

    _logger.LogInformation(
        "Inventory updated successfully. InventoryId: {InventoryId}, ProductId: {ProductId}, Operation: {Operation}, Quantity: {Quantity}, CurrentStock: {CurrentStock}.",
        inventory.Id,
        inventory.ProductId,
        request.Increase ? "Increase" : "Decrease",
        request.Quantity,
        inventory.Quantity.Value);

    return new Response<bool>(true);
}
}
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Inventory.Commands.DeleteById;

public class DeleteByIdInventoryCommandHandler
    : IRequestHandler<DeleteByIdInventoryCommand, Response<bool>>
{
    private readonly IInventoryService _inventoryService;
    private readonly IUnitOfWorkBase _unitOfWork;
    private readonly IValidator<DeleteByIdInventoryCommand> _validator;
    private readonly ILogger<DeleteByIdInventoryCommandHandler> _logger;

    public DeleteByIdInventoryCommandHandler
    (
        IInventoryService inventoryService,
        IUnitOfWorkBase unitOfWork,
        IValidator<DeleteByIdInventoryCommand> validator,
        ILogger<DeleteByIdInventoryCommandHandler> logger
    )
    {
        _inventoryService = inventoryService;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }


    public async Task<Response<bool>> Handle(
        DeleteByIdInventoryCommand request,
        CancellationToken ct)
    {
        _logger.LogInformation(
            "Starting inventory deletion. InventoryId: {InventoryId}.",
            request.InventoryId);

        var validationResult = await _validator
            .ValidateAsync(request, ct);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning(
                "Inventory deletion validation failed for InventoryId {InventoryId}. Errors: {Errors}",
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
                "Inventory deletion failed. InventoryId {InventoryId} was not found.",
                request.InventoryId);

            return new Response<bool>(
                false,
                "Inventory not found.");
        }

        var inventory = inventoryResponse.Data;

        await _inventoryService.DeleteByIdAsync(
            inventory.Id,
            ct);

        await _unitOfWork.SaveAsync(ct);

        _logger.LogInformation(
            "Inventory deleted successfully. InventoryId: {InventoryId}, ProductId: {ProductId}.",
            inventory.Id,
            inventory.ProductId);

        return new Response<bool>(true);
    }
}
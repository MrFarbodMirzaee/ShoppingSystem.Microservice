using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.ProductImage.Commands.Delete;

public class DeleteProductImageCommandHandler
    : IRequestHandler<DeleteProductImageCommand, Response<bool>>
{
    private readonly IProductImageService _productImageService;
    private readonly IUnitOfWorkBase _unitOfWork;
    private readonly IValidator<DeleteProductImageCommand> _validator;
    private readonly ILogger<DeleteProductImageCommandHandler> _logger;

    public DeleteProductImageCommandHandler
    (
        IProductImageService productImageService,
        IUnitOfWorkBase unitOfWork,
        IValidator<DeleteProductImageCommand> validator,
        ILogger<DeleteProductImageCommandHandler> logger
    )
    {
        _productImageService = productImageService;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Response<bool>> Handle(
        DeleteProductImageCommand request,
        CancellationToken ct)
    {
        _logger.LogInformation(
            "Starting product image deletion. ImageId: {ImageId}.",
            request.Id);

        var validationResult = await _validator
            .ValidateAsync(request, ct);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning(
                "Product image deletion validation failed. ImageId: {ImageId}. Errors: {Errors}",
                request.Id,
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

        var imageResponse = await _productImageService
            .GetByIdAsync(
                request.Id,
                ct);

        if (!imageResponse.Succeeded || imageResponse.Data is null)
        {
            _logger.LogWarning(
                "Product image deletion failed. ImageId {ImageId} was not found.",
                request.Id);

            return new Response<bool>(
                false,
                "Product image not found.");
        }

        var image = imageResponse.Data;

        await _productImageService.DeleteByIdAsync(
            image.Id,
            ct);

        await _unitOfWork.SaveAsync(ct);

        _logger.LogInformation(
            "Product image deleted successfully. ImageId: {ImageId}, ProductId: {ProductId}.",
            image.Id,
            image.ProductId);

        return new Response<bool>(true);
    }
}
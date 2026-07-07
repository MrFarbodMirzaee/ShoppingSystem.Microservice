using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Events.Product;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Product.Commands.Delete;

public class DeleteProductCommandHandler
    : IRequestHandler<DeleteProductCommand, Response<bool>>
{
    private readonly IProductService _productService;
    private readonly IUnitOfWorkBase _unitOfWork;
    private readonly IValidator<DeleteProductCommand> _validator;
    private readonly IMediator _mediator;
    private readonly ILogger<DeleteProductCommandHandler> _logger;

    public DeleteProductCommandHandler(
        IProductService productService,
        IUnitOfWorkBase unitOfWork,
        IValidator<DeleteProductCommand> validator,
        IMediator mediator,
        ILogger<DeleteProductCommandHandler> logger)
    {
        _productService = productService;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<Response<bool>> Handle(
        DeleteProductCommand request,
        CancellationToken ct)
    {
        _logger.LogInformation(
            "Starting product deletion. ProductId: {ProductId}.",
            request.ProductId);

        var validationResult = await _validator
            .ValidateAsync(request, ct);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning(
                "Product deletion validation failed for ProductId {ProductId}. Errors: {Errors}",
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

        var productResponse = await _productService
            .GetByIdAsync(request.ProductId, ct);

        if (!productResponse.Succeeded || productResponse.Data is null)
        {
            _logger.LogWarning(
                "Product deletion failed. ProductId {ProductId} was not found.",
                request.ProductId);

            return new Response<bool>(
                false,
                "Product not found.");
        }

        var product = productResponse.Data;

        await _productService.DeleteByIdAsync(
            product.Id,
            ct);

        await _unitOfWork.SaveAsync(ct);

        _logger.LogInformation(
            "Product deleted successfully. ProductId: {ProductId}, ProductName: {ProductName}.",
            product.Id,
            product.Name);

        await _mediator.Publish(
            new ProductDeletedEvent(product.Id),
            ct);

        _logger.LogInformation(
            "ProductDeletedEvent published successfully. ProductId: {ProductId}.",
            product.Id);

        return new Response<bool>(true);
    }
}
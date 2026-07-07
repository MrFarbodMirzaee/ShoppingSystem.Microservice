using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Events.Product;
using ShoppingSystem.Microservice.Domain.ValueObjects;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Product.Commands.Update;

public class UpdateProductCommandHandler
    : IRequestHandler<UpdateProductCommand, Response<bool>>
{
    private readonly IProductService _productService;
    private readonly IUnitOfWorkBase _unitOfWork;
    private readonly IValidator<UpdateProductCommand> _validator;
    private readonly ILogger<UpdateProductCommandHandler> _logger;

    private readonly IMediator _mediator;

    public UpdateProductCommandHandler(
        IProductService productService,
        IUnitOfWorkBase unitOfWork,
        IValidator<UpdateProductCommand> validator,
        IMediator mediator,
        ILogger<UpdateProductCommandHandler> logger)
    {
        _productService = productService;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<Response<bool>> Handle(
    UpdateProductCommand request,
    CancellationToken ct)
{
    _logger.LogInformation(
        "Starting product update. ProductId: {ProductId}.",
        request.ProductId);

    var validationResult = await _validator
        .ValidateAsync(request, ct);

    if (!validationResult.IsValid)
    {
        _logger.LogWarning(
            "Product update validation failed for ProductId {ProductId}. Errors: {Errors}",
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
            "Product update failed. ProductId {ProductId} was not found.",
            request.ProductId);

        return new Response<bool>(
            false,
            "Product not found.");
    }

    var product = productResponse.Data;

    var oldPrice = product.Price;

    product.Update(
        ProductName.Create(request.Name),
        request.Description,
        Money.Create(request.Price, request.Currency),
        request.CategoryId,
        request.IsAvailable);

    await _productService.UpdateAsync(product, ct);

    await _unitOfWork.SaveAsync(ct);

    _logger.LogInformation(
        "Product updated successfully. ProductId: {ProductId}, ProductName: {ProductName}.",
        product.Id,
        product.Name);

    if (!oldPrice.Equals(product.Price))
    {
        _logger.LogInformation(
            "Product price changed. ProductId: {ProductId}, OldPrice: {OldPrice}, NewPrice: {NewPrice}.",
            product.Id,
            oldPrice,
            product.Price);

        await _mediator.Publish(
            new ProductPriceChangedEvent(
                product.Id,
                Email.Create("admin@shopping.com"),
                product.Name,
                oldPrice,
                product.Price),
            ct);

        _logger.LogInformation(
            "ProductPriceChangedEvent published successfully. ProductId: {ProductId}.",
            product.Id);
    }

    return new Response<bool>(true);
}
}
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Events.Product;
using ShoppingSystem.Microservice.Domain.ValueObjects;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Product.Commands.Add;

public class AddProductCommandHandler
    : IRequestHandler<AddProductCommand, Response<bool>>
{
    private readonly IProductService _productService;
    private readonly IUnitOfWorkBase _unitOfWork;
    private readonly IValidator<AddProductCommand> _validator;
    private readonly IMediator _mediator;
    private readonly IUserQueryService _userQueryService;
    private readonly ILogger<AddProductCommandHandler> _logger;

    public AddProductCommandHandler(
        IProductService productService,
        IUnitOfWorkBase unitOfWork,
        IValidator<AddProductCommand> validator,
        IMediator mediator,
        IUserQueryService userQueryService,
        ILogger<AddProductCommandHandler> logger)
    {
        _productService = productService;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _mediator = mediator;
        _userQueryService = userQueryService;
        _logger = logger;
    }

    public async Task<Response<bool>> Handle(
    AddProductCommand request,
    CancellationToken ct)
{
    _logger.LogInformation(
        "Starting product creation. ProductName: {ProductName}, CategoryId: {CategoryId}, UserId: {UserId}.",
        request.Name,
        request.CategoryId,
        request.userId);

    var validationResult = await _validator
        .ValidateAsync(request, ct);

    if (!validationResult.IsValid)
    {
        _logger.LogWarning(
            "Product creation validation failed for ProductName {ProductName}. Errors: {Errors}",
            request.Name,
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

    var userExists = await _userQueryService
        .UserExistsAsync(request.userId, ct);

    if (!userExists)
    {
        _logger.LogWarning(
            "Product creation failed. UserId {UserId} does not exist.",
            request.userId);

        return new Response<bool>(
            false,
            "User does not exist.");
    }

    var exists = await _productService
        .ExistsByNameAsync(request.Name, ct);

    if (exists.Data)
    {
        _logger.LogWarning(
            "Product creation failed. Product with name '{ProductName}' already exists.",
            request.Name);

        return new Response<bool>(
            false,
            "Product already exists.");
    }

    var productName = ProductName.Create(request.Name);

    var price = Money.Create(
        request.Price,
        request.Currency);

    var product = Domain.Aggregates.Product.Product.Create(
        productName,
        request.Description,
        price,
        request.CategoryId);

    _productService.Add(product, ct);

    await _unitOfWork.SaveAsync(ct);

    _logger.LogInformation(
        "Product created successfully. ProductId: {ProductId}, ProductName: {ProductName}, CategoryId: {CategoryId}, Currency: {Currency}.",
        product.Id,
        product.Name,
        product.CategoryId,
        product.Price.Currency);

    await _mediator.Publish(
        new ProductCreatedEvent(
            product.Id,
            product.Name,
            product.Price,
            new List<string>
            {
                "admin@shopping.com"
            }),
        ct);

    _logger.LogInformation(
        "ProductCreatedEvent published successfully. ProductId: {ProductId}.",
        product.Id);

    return new Response<bool>(true);
}
}
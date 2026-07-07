using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.ProductImage.Commands.Add;

public class AddProductImageCommandHandler
    : IRequestHandler<AddProductImageCommand, Response<bool>>
{
    private readonly IProductImageService _productImageService;
    private readonly IUnitOfWorkBase _unitOfWork;
    private readonly IValidator<AddProductImageCommand> _validator;
    private readonly IUserQueryService _userQueryService;
    private readonly ILogger<AddProductImageCommandHandler> _logger;

    public AddProductImageCommandHandler
    (
        IProductImageService productImageService,
        IUnitOfWorkBase unitOfWork,
        IValidator<AddProductImageCommand> validator,
        IUserQueryService userQueryService,
        ILogger<AddProductImageCommandHandler> logger
    )
    {
        _productImageService = productImageService;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _userQueryService = userQueryService;
        _logger = logger;
    }

    public async Task<Response<bool>> Handle(
    AddProductImageCommand request,
    CancellationToken ct)
{
    _logger.LogInformation(
        "Starting product image upload. ProductId: {ProductId}, UserId: {UserId}.",
        request.ProductId,
        request.UserId);

    var validationResult = await _validator
        .ValidateAsync(request, ct);

    if (!validationResult.IsValid)
    {
        _logger.LogWarning(
            "Product image upload validation failed. ProductId: {ProductId}. Errors: {Errors}",
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

    var userExists = await _userQueryService
        .UserExistsAsync(request.UserId, ct);

    if (!userExists)
    {
        _logger.LogWarning(
            "Product image upload failed. UserId {UserId} does not exist.",
            request.UserId);

        return new Response<bool>(
            false,
            "User does not exist.");
    }

    if (request.File is null || request.File.Length == 0)
    {
        _logger.LogWarning(
            "Product image upload failed. Empty file received for ProductId {ProductId}.",
            request.ProductId);

        return new Response<bool>(
            false,
            "File cannot be empty.");
    }

    _logger.LogInformation(
        "Uploading product image. ProductId: {ProductId}, FileName: {FileName}, FileSize: {FileSize} bytes, IsMain: {IsMain}.",
        request.ProductId,
        request.File.FileName,
        request.File.Length,
        request.IsMain);

    var result = await _productImageService.UploadAsync(
        request.ProductId,
        request.File,
        request.IsMain,
        ct);

    if (result is null)
    {
        _logger.LogError(
            "Product image upload failed. ProductId: {ProductId}, FileName: {FileName}.",
            request.ProductId,
            request.File.FileName);

        return new Response<bool>(
            false,
            "Image upload failed.");
    }

    await _unitOfWork.SaveAsync(ct);

    _logger.LogInformation(
        "Product image uploaded successfully. ProductId: {ProductId}, FileName: {FileName}, IsMain: {IsMain}.",
        request.ProductId,
        request.File.FileName,
        request.IsMain);

    return new Response<bool>(true);
}
}
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Category.Commands.Add;

public class AddCategoryCommandHandler
    : IRequestHandler<AddCategoryCommand, Response<bool>>
{
    private readonly ICategoryService _categoryService;
    private readonly IUnitOfWorkBase _unitOfWork;
    private readonly IValidator<AddCategoryCommand> _validator;
    private readonly IUserQueryService _userQueryService;
    private readonly ILogger<AddCategoryCommandHandler> _logger;

    public AddCategoryCommandHandler
    (
        ICategoryService categoryService,
        IUnitOfWorkBase unitOfWork,
        IValidator<AddCategoryCommand> validator,
        IUserQueryService userQueryService,
        ILogger<AddCategoryCommandHandler> logger
    )
    {
        _categoryService = categoryService;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
        _userQueryService = userQueryService;
    }

   public async Task<Response<bool>> Handle(
    AddCategoryCommand request,
    CancellationToken ct)
{
    _logger.LogInformation(
        "Starting category creation. Name: {CategoryName}.",
        request.Name);

    var validationResult = await _validator
        .ValidateAsync(request, ct);

    if (!validationResult.IsValid)
    {
        _logger.LogWarning(
            "Category creation validation failed. Name: {CategoryName}. Errors: {Errors}",
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
        .UserExistsAsync(request.UserId, ct);

    if (!userExists)
    {
        _logger.LogWarning(
            "Category creation failed. User {UserId} does not exist.",
            request.UserId);

        return new Response<bool>(
            false,
            "User does not exist.");
    }

    var exists = await _categoryService
        .ExistsByNameAsync(
            request.Name,
            ct);

    if (exists.Data)
    {
        _logger.LogWarning(
            "Category creation failed. Category '{CategoryName}' already exists.",
            request.Name);

        return new Response<bool>(
            false,
            "Category already exists.");
    }

    if (request.ParentCategoryId.HasValue)
    {
        var parent = await _categoryService
            .GetByIdAsync(
                request.ParentCategoryId.Value,
                ct);

        if (!parent.Succeeded || parent.Data is null)
        {
            _logger.LogWarning(
                "Category creation failed. ParentCategoryId {ParentCategoryId} was not found.",
                request.ParentCategoryId.Value);

            return new Response<bool>(
                false,
                "Parent category not found.");
        }
    }

    var category = new Domain.Aggregates.Product.Category(
        request.Name,
        request.Description,
        request.ParentCategoryId);

    _categoryService.Add(category, ct);

    await _unitOfWork.SaveAsync(ct);

    _logger.LogInformation(
        "Category created successfully. CategoryId: {CategoryId}, Name: {CategoryName}.",
        category.Id,
        category.Name);

    return new Response<bool>(true);
}
}
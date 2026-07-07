using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Category.Commands.Update;

public class UpdateCategoryCommandHandler
    : IRequestHandler<UpdateCategoryCommand, Response<bool>>
{
    private readonly ICategoryService _categoryService;
    private readonly IUnitOfWorkBase _unitOfWork;
    private readonly IValidator<UpdateCategoryCommand> _validator;
    private readonly ILogger<UpdateCategoryCommandHandler> _logger;

    public UpdateCategoryCommandHandler
    (
        ICategoryService categoryService,
        IUnitOfWorkBase unitOfWork,
        IValidator<UpdateCategoryCommand> validator,
        ILogger<UpdateCategoryCommandHandler> logger
    )
    {
        _categoryService = categoryService;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Response<bool>> Handle(
    UpdateCategoryCommand request,
    CancellationToken ct)
{
    _logger.LogInformation(
        "Starting category update. CategoryId: {CategoryId}.",
        request.CategoryId);

    var validationResult = await _validator
        .ValidateAsync(request, ct);

    if (!validationResult.IsValid)
    {
        _logger.LogWarning(
            "Category update validation failed for CategoryId {CategoryId}. Errors: {Errors}",
            request.CategoryId,
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

    var categoryResponse = await _categoryService
        .GetByIdAsync(request.CategoryId, ct);

    if (!categoryResponse.Succeeded || categoryResponse.Data is null)
    {
        _logger.LogWarning(
            "Category update failed. CategoryId {CategoryId} was not found.",
            request.CategoryId);

        return new Response<bool>(
            false,
            "Category not found.");
    }

    var category = categoryResponse.Data;

    var exists = await _categoryService
        .ExistsByNameAsync(request.Name, ct);

    if (exists.Succeeded &&
        exists.Data &&
        !string.Equals(
            category.Name,
            request.Name,
            StringComparison.OrdinalIgnoreCase))
    {
        _logger.LogWarning(
            "Category update failed. A category with the name '{CategoryName}' already exists.",
            request.Name);

        return new Response<bool>(
            false,
            "A category with this name already exists.");
    }

    category.Update(
        request.Name,
        request.Description);

    await _categoryService.UpdateAsync(category, ct);

    await _unitOfWork.SaveAsync(ct);

    _logger.LogInformation(
        "Category updated successfully. CategoryId: {CategoryId}, Name: {CategoryName}.",
        category.Id,
        category.Name);

    return new Response<bool>(true);
}
}
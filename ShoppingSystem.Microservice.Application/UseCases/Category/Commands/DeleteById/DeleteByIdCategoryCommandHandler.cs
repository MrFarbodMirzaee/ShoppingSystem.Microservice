using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Category.Commands.DeleteById;

public class DeleteByIdCategoryCommandHandler
    : IRequestHandler<DeleteByIdCategoryCommand, Response<bool>>
{
    private readonly ICategoryService _categoryService;
    private readonly IUnitOfWorkBase _unitOfWork;
    private readonly IValidator<DeleteByIdCategoryCommand> _validator;
    private readonly ILogger<DeleteByIdCategoryCommandHandler> _logger;

    public DeleteByIdCategoryCommandHandler
    (
        ICategoryService categoryService,
        IUnitOfWorkBase unitOfWork,
        IValidator<DeleteByIdCategoryCommand> validator,
        ILogger<DeleteByIdCategoryCommandHandler> logger
    )
    {
        _categoryService = categoryService;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Response<bool>> Handle(
        DeleteByIdCategoryCommand request,
        CancellationToken ct)
    {
        _logger.LogInformation(
            "Starting category deletion. CategoryId: {CategoryId}.",
            request.CategoryId);

        var validationResult = await _validator
            .ValidateAsync(request, ct);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning(
                "Category deletion validation failed for CategoryId {CategoryId}. Errors: {Errors}",
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
                "Category deletion failed. CategoryId {CategoryId} was not found.",
                request.CategoryId);

            return new Response<bool>(
                false,
                "Category not found.");
        }

        await _categoryService.DeleteByIdAsync(
            categoryResponse.Data.Id,
            ct);

        await _unitOfWork.SaveAsync(ct);

        _logger.LogInformation(
            "Category deleted successfully. CategoryId: {CategoryId}.",
            request.CategoryId);

        return new Response<bool>(true);
    }
}
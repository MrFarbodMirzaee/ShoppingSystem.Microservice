using FluentValidation;
using ShoppingSystem.Microservice.Resource.Messages;

namespace ShoppingSystem.Microservice.Application.UseCases.Category.Commands.DeleteById;

public sealed class DeleteByIdCategoryCommandValidator
    : AbstractValidator<DeleteByIdCategoryCommand>
{
    public DeleteByIdCategoryCommandValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.CategoryId)));
    }
}
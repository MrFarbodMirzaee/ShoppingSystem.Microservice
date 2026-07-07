using FluentValidation;
using ShoppingSystem.Microservice.Resource.Messages;

namespace ShoppingSystem.Microservice.Application.UseCases.Category.Commands.Add;

public sealed class AddCategoryCommandValidator
    : AbstractValidator<AddCategoryCommand>
{
    public AddCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.Name)))
            .MaximumLength(100)
            .WithMessage(prop => string.Format(
                Errors.Maximum,
                nameof(prop.Name),
                100));

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage(prop => string.Format(
                Errors.Maximum,
                nameof(prop.Description),
                500));
    }
}
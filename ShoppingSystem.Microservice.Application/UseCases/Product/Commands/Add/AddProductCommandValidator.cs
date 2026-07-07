using FluentValidation;
using ShoppingSystem.Microservice.Resource.Messages;

namespace ShoppingSystem.Microservice.Application.UseCases.Product.Commands.Add;

public sealed class AddProductCommandValidator
    : AbstractValidator<AddProductCommand>
{
    public AddProductCommandValidator()
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
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.Description)))
            .MaximumLength(500)
            .WithMessage(prop => string.Format(
                Errors.Maximum,
                nameof(prop.Description),
                500));

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage(prop => string.Format(
                Errors.GreaterThan,
                nameof(prop.Price),
                0));

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.CategoryId)));

        RuleFor(x => x.Currency)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.Currency)))
            .MaximumLength(3)
            .WithMessage(prop => string.Format(
                Errors.Maximum,
                nameof(prop.Currency),
                3));
    }
}
using FluentValidation;
using ShoppingSystem.Microservice.Resource.Messages;

namespace ShoppingSystem.Microservice.Application.UseCases.Inventory.Commands.Update;

public sealed class UpdateInventoryCommandValidator
    : AbstractValidator<UpdateInventoryCommand>
{
    public UpdateInventoryCommandValidator()
    {
        RuleFor(x => x.InventoryId)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.InventoryId)));

        RuleFor(x => x.Quantity)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.Quantity)))
            .GreaterThan((byte)0)
            .WithMessage(prop => string.Format(
                Errors.Minimum,
                nameof(prop.Quantity),
                1));

        RuleFor(x => x.Increase)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.Increase)));
    }
}
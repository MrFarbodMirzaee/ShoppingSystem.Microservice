using FluentValidation;
using ShoppingSystem.Microservice.Resource.Messages;

namespace ShoppingSystem.Microservice.Application.UseCases.Inventory.Commands.Add;

public sealed class AddInventoryCommandValidator
    : AbstractValidator<AddInventoryCommand>
{
    public AddInventoryCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.ProductId)));

        RuleFor(x => x.Quantity)
            .GreaterThan((byte)0)
            .WithMessage(prop => string.Format(
                Errors.Minimum,
                nameof(prop.Quantity),
                1));
    }
}
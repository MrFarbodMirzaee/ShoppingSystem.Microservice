using FluentValidation;
using ShoppingSystem.Microservice.Resource.Messages;

namespace ShoppingSystem.Microservice.Application.UseCases.Inventory.Commands.DeleteById;

public sealed class DeleteByIdInventoryCommandValidator
    : AbstractValidator<DeleteByIdInventoryCommand>
{
    public DeleteByIdInventoryCommandValidator()
    {
        RuleFor(x => x.InventoryId)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.InventoryId)));
    }
}
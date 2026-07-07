using FluentValidation;
using ShoppingSystem.Microservice.Resource.Messages;

namespace ShoppingSystem.Microservice.Application.UseCases.Inventory.Queries.GetById;

public sealed class GetByIdInventoryQueryValidator
    : AbstractValidator<GetByIdInventoryQuery>
{
    public GetByIdInventoryQueryValidator()
    {
        RuleFor(x => x.InventoryId)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.InventoryId)));
    }
}
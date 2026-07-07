using FluentValidation;
using ShoppingSystem.Microservice.Resource.Messages;

namespace ShoppingSystem.Microservice.Application.UseCases.Inventory.Queries.GetLowStock;

public sealed class GetLowStockInventoryQueryValidator
    : AbstractValidator<GetLowStockInventoryQuery>
{
    public GetLowStockInventoryQueryValidator()
    {
        RuleFor(x => x.lowStockInventories)
            .GreaterThan(0)
            .WithMessage(prop => string.Format(
                Errors.GreaterThan,
                nameof(prop.lowStockInventories),
                0))
            .LessThanOrEqualTo(255)
            .WithMessage(prop => string.Format(
                Errors.Maximum,
                nameof(prop.lowStockInventories),
                255));
    }
}
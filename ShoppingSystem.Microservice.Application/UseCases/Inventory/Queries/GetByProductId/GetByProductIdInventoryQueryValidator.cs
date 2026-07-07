using FluentValidation;
using ShoppingSystem.Microservice.Resource.Messages;

namespace ShoppingSystem.Microservice.Application.UseCases.Inventory.Queries.GetByProductId;

public sealed class GetByProductIdInventoryQueryValidator
    : AbstractValidator<GetByProductIdInventoryQuery>
{
    public GetByProductIdInventoryQueryValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.ProductId)));
    }
}
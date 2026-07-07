using FluentValidation;
using ShoppingSystem.Microservice.Resource.Messages;

namespace ShoppingSystem.Microservice.Application.UseCases.Product.Queries.GetByCategory;

public sealed class GetByCategoryProductQueryValidator
    : AbstractValidator<GetByCategoryProductsQuery>
{
    public GetByCategoryProductQueryValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.CategoryId)));
    }
}
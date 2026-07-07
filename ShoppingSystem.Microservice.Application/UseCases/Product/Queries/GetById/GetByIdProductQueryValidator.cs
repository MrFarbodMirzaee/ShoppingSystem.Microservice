using FluentValidation;
using ShoppingSystem.Microservice.Resource.Messages;

namespace ShoppingSystem.Microservice.Application.UseCases.Product.Queries.GetById;

public sealed class GetByIdProductQueryValidator
    : AbstractValidator<GetByIdProductQuery>
{
    public GetByIdProductQueryValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.ProductId)));
    }
}
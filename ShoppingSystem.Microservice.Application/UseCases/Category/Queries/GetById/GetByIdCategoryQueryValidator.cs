using FluentValidation;
using ShoppingSystem.Microservice.Resource.Messages;

namespace ShoppingSystem.Microservice.Application.UseCases.Category.Queries.GetById;

public sealed class GetByIdCategoryQueryValidator
    : AbstractValidator<GetByIdCategoryQuery>
{
    public GetByIdCategoryQueryValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.CategoryId)));
    }
}
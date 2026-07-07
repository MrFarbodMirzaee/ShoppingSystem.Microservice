using FluentValidation;
using ShoppingSystem.Microservice.Resource.Messages;

namespace ShoppingSystem.Microservice.Application.UseCases.Category.Queries.GetByName;

public sealed class GetByNameCategoryQueryValidator
    : AbstractValidator<GetByNameCategoryQuery>
{
    public GetByNameCategoryQueryValidator()
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
    }
}
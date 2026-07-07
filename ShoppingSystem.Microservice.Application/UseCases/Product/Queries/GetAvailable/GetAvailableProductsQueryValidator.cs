using FluentValidation;
using ShoppingSystem.Microservice.Resource.Messages;

namespace ShoppingSystem.Microservice.Application.UseCases.Product.Queries.GetAvailable;

public class GetAvailableProductsQueryValidator 
    : AbstractValidator<GetAvailableProductsQuery>
{
    public GetAvailableProductsQueryValidator()
    {
        RuleFor(x => x.QueryCriteriaRequestDto.PageNumber)
            .NotNull()
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.QueryCriteriaRequestDto.PageNumber)));

        RuleFor(x => x.QueryCriteriaRequestDto.PageSize)
            .NotNull()
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.QueryCriteriaRequestDto.PageSize)));
    }
}
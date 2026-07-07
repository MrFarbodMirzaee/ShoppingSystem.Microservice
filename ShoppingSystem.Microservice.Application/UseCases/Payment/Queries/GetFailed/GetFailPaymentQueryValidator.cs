using FluentValidation;
using ShoppingSystem.Microservice.Resource.Messages;

namespace ShoppingSystem.Microservice.Application.UseCases.Payment.Queries.GetFailed;

public class GetFailPaymentQueryValidator 
    : AbstractValidator<GetFailPaymentQuery>
{
    public GetFailPaymentQueryValidator()
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
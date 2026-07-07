using FluentValidation;
using ShoppingSystem.Microservice.Resource.Messages;

namespace ShoppingSystem.Microservice.Application.UseCases.ProductImage.Queries.GetUrl;

public class GetUrlProductImageQueryValidator
    : AbstractValidator<GetUrlProductImageQuery>
{
    public GetUrlProductImageQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.Id)))
            .NotEqual(Guid.Empty)
            .WithMessage(prop => string.Format(
                Errors.Invalid,
                nameof(prop.Id)));
    }
}
using FluentValidation;
using ShoppingSystem.Microservice.Resource.Messages;

namespace ShoppingSystem.Microservice.Application.UseCases.ProductImage.Queries.Download;

public class DownloadProductImageQueryValidator
    : AbstractValidator<DownloadProductImageQuery>
{
    public DownloadProductImageQueryValidator()
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
using FluentValidation;
using ShoppingSystem.Microservice.Resource.Messages;

namespace ShoppingSystem.Microservice.Application.UseCases.ProductImage.Commands.Add;

public sealed class AddProductImageCommandValidator
    : AbstractValidator<AddProductImageCommand>
{
    private static readonly string[] AllowedContentTypes =
    {
        "image/jpeg",
        "image/png",
        "image/webp",
        "image/heic",
        "image/heif"
    };


    public AddProductImageCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.ProductId)));


        RuleFor(x => x.File)
            .NotNull()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.File)));


        RuleFor(x => x.File.Length)
            .GreaterThan(0)
            .WithMessage(prop => string.Format(
                Errors.GreaterThan,
                nameof(prop.File.Length),
                0))
            .LessThanOrEqualTo(5 * 1024 * 1024)
            .WithMessage(prop => string.Format(
                Errors.Maximum,
                nameof(prop.File.Length),
                "5MB"));


        RuleFor(x => x.File.ContentType)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.File.ContentType)))
            .Must(BeValidImageType)
            .WithMessage(prop => string.Format(
                Errors.InvalidFormat,
                nameof(prop.File.ContentType)));


        RuleFor(x => x.File.FileName)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.File.FileName)))
            .Must(BeValidFileName)
            .WithMessage(prop => string.Format(
                Errors.InvalidFormat,
                nameof(prop.File.FileName)));
    }


    private static bool BeValidImageType(string contentType)
    {
        return AllowedContentTypes
            .Contains(
                contentType.ToLowerInvariant()
            );
    }


    private static bool BeValidFileName(string fileName)
    {
        return fileName.IndexOfAny(
            Path.GetInvalidFileNameChars()
        ) < 0;
    }
}
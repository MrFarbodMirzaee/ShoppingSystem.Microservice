using FluentValidation;
using ShoppingSystem.Microservice.Resource.Messages;

namespace ShoppingSystem.Microservice.Application.UseCases.Address.Commands.Add;

public class AddAddressCommandRequestValidator
    : AbstractValidator<AddAddressRequestCommand>
{
    public AddAddressCommandRequestValidator()
    {
        RuleFor(x => x.Street)
            .NotEmpty()
            .NotNull()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.Street)))
            .MaximumLength(200)
            .WithMessage(prop => string.Format(
                Errors.Maximum,
                nameof(prop.Street),
                200))
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.Street)));

        RuleFor(x => x.City)
            .NotEmpty()
            .NotNull()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.City)))
            .MaximumLength(100)
            .WithMessage(prop => string.Format(
                Errors.Maximum,
                nameof(prop.City),
                100))
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.City)));

        RuleFor(x => x.State)
            .MaximumLength(100)
            .WithMessage(prop => string.Format(
                Errors.Maximum,
                nameof(prop.State),
                100));

        RuleFor(x => x.Country)
            .NotEmpty()
            .NotNull()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.Country)))
            .MaximumLength(100)
            .WithMessage(prop => string.Format(
                Errors.Maximum,
                nameof(prop.Country),
                100))
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.Country)));

        RuleFor(x => x.PostalCode)
            .NotEmpty()
            .NotNull()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.PostalCode)))
            .MaximumLength(20)
            .WithMessage(prop => string.Format(
                Errors.Maximum,
                nameof(prop.PostalCode),
                20))
            .Matches(@"^[A-Za-z0-9\- ]+$")
            .WithMessage(prop => string.Format(
                Errors.InvalidFormat,
                nameof(prop.PostalCode)));
    }
}
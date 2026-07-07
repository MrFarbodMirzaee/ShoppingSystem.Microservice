using FluentValidation;
using ShoppingSystem.Microservice.Resource.Messages;

namespace ShoppingSystem.Microservice.Application.UseCases.Address.Commands.Update;

public class UpdateAddressCommandRequestValidator
    : AbstractValidator<UpdateAddressCommandRequest>
{
    public UpdateAddressCommandRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.Id)));

        RuleFor(x => x.Street)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.Street)))
            .MaximumLength(200)
            .WithMessage(prop => string.Format(
                Errors.Maximum,
                nameof(prop.Street),
                200));

        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.City)))
            .MaximumLength(100)
            .WithMessage(prop => string.Format(
                Errors.Maximum,
                nameof(prop.City),
                100));

        RuleFor(x => x.State)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.State)))
            .MaximumLength(100)
            .WithMessage(prop => string.Format(
                Errors.Maximum,
                nameof(prop.State),
                100));

        RuleFor(x => x.Country)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.Country)))
            .MaximumLength(100)
            .WithMessage(prop => string.Format(
                Errors.Maximum,
                nameof(prop.Country),
                100));

        RuleFor(x => x.PostalCode)
            .NotEmpty()
            .WithMessage(prop => string.Format(
                Errors.Required,
                nameof(prop.PostalCode)))
            .Matches(@"^\d{10}$")
            .WithMessage(prop => string.Format(
                Errors.InvalidFormat,
                nameof(prop.PostalCode)));
    }
}
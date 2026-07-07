using MediatR;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Address.Commands.Update;

public sealed record UpdateAddressCommandRequest(
    Guid Id,
    string Street,
    string City,
    string State,
    string Country,
    string PostalCode
) : IRequest<Response<bool>>;
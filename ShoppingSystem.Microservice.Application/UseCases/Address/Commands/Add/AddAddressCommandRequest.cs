using MediatR;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Address.Commands.Add;

public sealed record AddAddressRequestCommand(
    string Street,
    string City,
    string State,
    string Country,
    string PostalCode,
    Guid UserId)
    : IRequest<Response<bool>>;
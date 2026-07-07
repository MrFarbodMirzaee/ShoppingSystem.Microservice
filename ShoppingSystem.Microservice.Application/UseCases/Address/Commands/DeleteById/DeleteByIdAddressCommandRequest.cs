using MediatR;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Address.Commands.DeleteById;

public sealed record DeleteByIdAddressCommandRequest(
    Guid Id)
    : IRequest<Response<bool>>;
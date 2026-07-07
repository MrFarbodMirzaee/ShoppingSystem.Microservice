using MediatR;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Payment.Commands.Add;

public sealed record AddPaymentCommand(
    Guid OrderId,
    decimal Amount,
    string Currency,
    Guid UserId
) : IRequest<Response<bool>>;
using MediatR;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Payment.Commands.Fail;

public sealed record FailPaymentCommand(
    Guid PaymentId,
    string Reason,
    Guid userId
) : IRequest<Response<bool>>;
using MediatR;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Payment.Commands.Refund;

public sealed record RefundPaymentCommand(
    Guid PaymentId,
    Guid UserId,
    string Message
) : IRequest<Response<bool>>;
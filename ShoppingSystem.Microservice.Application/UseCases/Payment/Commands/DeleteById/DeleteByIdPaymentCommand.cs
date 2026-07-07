using MediatR;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Payment.Commands.DeleteById;

public sealed record DeleteByIdPaymentCommand(
    Guid PaymentId
) : IRequest<Response<bool>>;
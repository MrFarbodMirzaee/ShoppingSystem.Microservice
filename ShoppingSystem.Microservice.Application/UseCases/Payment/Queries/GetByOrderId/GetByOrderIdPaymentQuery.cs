using MediatR;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Payment.Queries.GetByOrderId;

public sealed record GetByOrderIdPaymentQuery(
    Guid OrderId
) : IRequest<Response<PaymentResponseDto>>;
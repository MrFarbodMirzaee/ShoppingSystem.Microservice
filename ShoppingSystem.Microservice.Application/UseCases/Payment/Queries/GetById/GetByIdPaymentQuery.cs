using MediatR;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Payment.Queries.GetById;

public sealed record GetByIdPaymentQuery(
    Guid PaymentId
) : IRequest<Response<PaymentResponseDto>>;
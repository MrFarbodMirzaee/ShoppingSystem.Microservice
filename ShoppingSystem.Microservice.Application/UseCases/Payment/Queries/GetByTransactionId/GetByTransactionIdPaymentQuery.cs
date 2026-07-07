using MediatR;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Payment.Queries.GetByTransactionId;

public sealed record GetByTransactionIdPaymentQuery(
    string TransactionId
) : IRequest<Response<PaymentResponseDto>>;
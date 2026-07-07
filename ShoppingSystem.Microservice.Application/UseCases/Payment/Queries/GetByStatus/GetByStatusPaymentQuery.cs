using MediatR;
using ShoppingSystem.Microservice.Application.Common;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Domain.Enums;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Payment.Queries.GetByStatus;

public sealed record GetByStatusPaymentQuery(
    PaymentStatus Status,
    QueryCriteriaRequestDto QueryCriteriaRequestDto
) : IRequest<Response<PagedResult<PaymentResponseDto>>>;
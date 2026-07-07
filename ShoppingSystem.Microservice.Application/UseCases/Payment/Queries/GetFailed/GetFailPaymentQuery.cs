using MediatR;
using ShoppingSystem.Microservice.Application.Common;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Payment.Queries.GetFailed;

public sealed record GetFailPaymentQuery(QueryCriteriaRequestDto QueryCriteriaRequestDto)
    : IRequest<Response<PagedResult<PaymentResponseDto>>>;
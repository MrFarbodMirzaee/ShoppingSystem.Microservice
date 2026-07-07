using MediatR;
using ShoppingSystem.Microservice.Application.Common;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Payment.Queries.GetAll;

public sealed record GetAllPaymentQuery(QueryCriteriaRequestDto QueryCriteria)
    : IRequest<Response<PagedResult<PaymentResponseDto>>>;
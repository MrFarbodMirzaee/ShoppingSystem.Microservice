using AutoMapper;
using MediatR;
using ShoppingSystem.Microservice.Application.Common;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Enums;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Payment.Queries.GetSuccessful;

public sealed class GetSuccessfulPaymentQueryHandler
    : IRequestHandler<GetSuccessfulPaymentQuery,
        Response<PagedResult<PaymentResponseDto>>>
{
    private readonly IPaymentService _paymentService;
    private readonly IMapper _mapper;
    public GetSuccessfulPaymentQueryHandler(
        IPaymentService paymentService,
        IMapper mapper)
    {
        _paymentService = paymentService;
        _mapper = mapper;
    }

    public async Task<Response<PagedResult<PaymentResponseDto>>> Handle(
        GetSuccessfulPaymentQuery request,
        CancellationToken ct)
    {

        var criteria = _mapper.Map<QueryCriteria>(
            request.QueryCriteriaRequestDto
        );

        var result = await _paymentService.GetByStatusAsync(
            criteria,
            PaymentStatus.Successful,
            ct
        );

        if (!result.Succeeded || result.Data is null)
        {
            return new Response<PagedResult<PaymentResponseDto>>
            {
                Succeeded = false,
                Message = result.Message ?? "No successful payments found.",
                Errors = result.Errors
            };
        }

        var mappedItems = _mapper.Map<List<PaymentResponseDto>>(
            result.Data.Items
        );

        var pagedResponse = new PagedResult<PaymentResponseDto>
        {
            Items = mappedItems,
            TotalCount = result.Data.TotalCount,
            PageNumber = result.Data.PageNumber,
            PageSize = result.Data.PageSize
        };

        return new Response<PagedResult<PaymentResponseDto>>(pagedResponse);
    }
}
using AutoMapper;
using MediatR;
using ShoppingSystem.Microservice.Application.Common;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Payment.Queries.GetAll;

public class GetAllPaymentQueryHandler
    : IRequestHandler<GetAllPaymentQuery, 
    Response<PagedResult<PaymentResponseDto>>>
{
    private readonly IPaymentService _paymentService;
    private readonly IMapper _mapper;

    public GetAllPaymentQueryHandler
    (
        IPaymentService paymentService,
        IMapper mapper
    )
    {
        _paymentService = paymentService;
        _mapper = mapper;
    }

    public async Task<Response<PagedResult<PaymentResponseDto>>> Handle(
        GetAllPaymentQuery request,
        CancellationToken ct)
    {
        var mappedCriteria = _mapper.Map<QueryCriteria>(request.QueryCriteria);

        var paymentsResponse = await _paymentService
            .GetAllAsync(mappedCriteria, ct);

        if (!paymentsResponse.Succeeded || paymentsResponse.Data is null)
            return new Response<PagedResult<PaymentResponseDto>>(
                "It couldn't retrieve payments.",
                paymentsResponse.Errors);

        var mappedItems = _mapper.Map<List<PaymentResponseDto>>(
            paymentsResponse.Data.Items);

        var pagedResponse = new PagedResult<PaymentResponseDto>
        {
            Items = mappedItems,
            TotalCount = paymentsResponse.Data.TotalCount,
            PageNumber = paymentsResponse.Data.PageNumber,
            PageSize = paymentsResponse.Data.PageSize
        };

        return new Response<PagedResult<PaymentResponseDto>>(pagedResponse);
    }
}
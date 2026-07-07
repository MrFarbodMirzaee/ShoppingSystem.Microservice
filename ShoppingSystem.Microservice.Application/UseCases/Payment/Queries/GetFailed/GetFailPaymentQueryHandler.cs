using AutoMapper;
using FluentValidation;
using MediatR;
using ShoppingSystem.Microservice.Application.Common;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Enums;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Payment.Queries.GetFailed;

public sealed class GetFailPaymentQueryHandler
    : IRequestHandler<GetFailPaymentQuery,
        Response<PagedResult<PaymentResponseDto>>>
{
    private readonly IPaymentService _paymentService;
    private readonly IMapper _mapper;
    private readonly IValidator<GetFailPaymentQuery> _validator;

    public GetFailPaymentQueryHandler(
        IPaymentService paymentService,
        IMapper mapper,
        IValidator<GetFailPaymentQuery> validator)
    {
        _paymentService = paymentService;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<Response<PagedResult<PaymentResponseDto>>> Handle(
        GetFailPaymentQuery request,
        CancellationToken ct)
    {
        var validationResult = await _validator.ValidateAsync(request, ct);

        if (!validationResult.IsValid)
        {
            return new Response<PagedResult<PaymentResponseDto>>
            {
                Succeeded = false,
                Message = "Validation failed.",
                Errors = validationResult.Errors
                    .Select(x => x.ErrorMessage)
                    .ToList()
            };
        }

        var criteria = _mapper.Map<QueryCriteria>(
            request.QueryCriteriaRequestDto
        );

        var result = await _paymentService.GetByStatusAsync(
            criteria,
            PaymentStatus.Failed,
            ct
        );

        if (!result.Succeeded || result.Data is null)
        {
            return new Response<PagedResult<PaymentResponseDto>>
            {
                Succeeded = false,
                Message = result.Message ?? "No failed payments found.",
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
using AutoMapper;
using FluentValidation;
using MediatR;
using ShoppingSystem.Microservice.Application.Common;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Order.Queries.GetByStatus;

public sealed class GetByStatusOrderQueryHandler
    : IRequestHandler<GetByStatusOrderQuery,
        Response<PagedResult<OrderResponseDto>>>
{
    private readonly IOrderService _orderService;
    private readonly IMapper _mapper;
    private readonly IValidator<GetByStatusOrderQuery> _validator;

    public GetByStatusOrderQueryHandler(
        IOrderService orderService,
        IMapper mapper,
        IValidator<GetByStatusOrderQuery> validator)
    {
        _orderService = orderService;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<Response<PagedResult<OrderResponseDto>>> Handle(
        GetByStatusOrderQuery request,
        CancellationToken ct)
    {
        var validationResult = await _validator.ValidateAsync(request, ct);

        if (!validationResult.IsValid)
        {
            return new Response<PagedResult<OrderResponseDto>>
            {
                Succeeded = false,
                Message = "Validation failed.",
                Errors = validationResult.Errors
                    .Select(x => x.ErrorMessage)
                    .ToList()
            };
        }

        // ✅ Map QueryCriteria
        var criteria = _mapper.Map<QueryCriteria>(
            request.QueryCriteriaRequestDto
        );

        var result = await _orderService.GetByStatusAsync(
            criteria,
            request.Status,
            ct
        );

        if (!result.Succeeded || result.Data is null)
        {
            return new Response<PagedResult<OrderResponseDto>>
            {
                Succeeded = false,
                Message = result.Message ?? "No orders found with this status.",
                Errors = result.Errors
            };
        }

        // ✅ Map paged items
        var mappedItems = _mapper.Map<List<OrderResponseDto>>(
            result.Data.Items
        );

        var pagedResponse = new PagedResult<OrderResponseDto>
        {
            Items = mappedItems,
            TotalCount = result.Data.TotalCount,
            PageNumber = result.Data.PageNumber,
            PageSize = result.Data.PageSize
        };

        return new Response<PagedResult<OrderResponseDto>>(pagedResponse);
    }
}
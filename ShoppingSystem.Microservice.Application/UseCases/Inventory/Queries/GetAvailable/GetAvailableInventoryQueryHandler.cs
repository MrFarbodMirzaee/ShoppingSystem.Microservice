using AutoMapper;
using MediatR;
using ShoppingSystem.Microservice.Application.Common;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Inventory.Queries.GetAvailable;

public sealed class GetAvailableInventoryQueryHandler
    : IRequestHandler<GetAvailableInventoryQuery,
        Response<PagedResult<InventoryResponseDto>>>
{
    private readonly IInventoryService _inventoryService;
    private readonly IMapper _mapper;

    public GetAvailableInventoryQueryHandler(
        IInventoryService inventoryService,
        IMapper mapper)
    {
        _inventoryService = inventoryService;
        _mapper = mapper;
    }

    public async Task<Response<PagedResult<InventoryResponseDto>>> Handle(
        GetAvailableInventoryQuery request,
        CancellationToken ct)
    {
        var mappedCriteria = _mapper.Map<QueryCriteria>(request.QueryCriteriaRequestDto);

        var result = await _inventoryService.GetAvailableInventoryAsync(
            mappedCriteria,
            ct);

        if (!result.Succeeded || result.Data is null)
        {
            return new Response<PagedResult<InventoryResponseDto>>
            {
                Succeeded = false,
                Message = result.Message ?? "No available inventories found.",
                Errors = result.Errors
            };
        }

        var mappedItems = _mapper.Map<List<InventoryResponseDto>>(result.Data.Items);

        var pagedResponse = new PagedResult<InventoryResponseDto>
        {
            Items = mappedItems,
            TotalCount = result.Data.TotalCount,
            PageNumber = result.Data.PageNumber,
            PageSize = result.Data.PageSize
        };

        return new Response<PagedResult<InventoryResponseDto>>(pagedResponse);
    }
}
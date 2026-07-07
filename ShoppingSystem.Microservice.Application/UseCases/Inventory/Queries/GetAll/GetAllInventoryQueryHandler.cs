using AutoMapper;
using MediatR;
using ShoppingSystem.Microservice.Application.Common;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Inventory.Queries.GetAll;

public class GetAllInventoryQueryHandler
    : IRequestHandler<GetAllInventoryQuery, 
    Response<PagedResult<InventoryResponseDto>>>
{
    private readonly IInventoryService _inventoryService;
    private readonly IMapper _mapper;

    public GetAllInventoryQueryHandler
    (
        IInventoryService inventoryService,
        IMapper mapper
    )
    {
        _inventoryService = inventoryService;
        _mapper = mapper;
    }


    public async Task<Response<PagedResult<InventoryResponseDto>>> Handle(
        GetAllInventoryQuery request,
        CancellationToken ct)
    {
        var mappedCriteria = _mapper.Map<QueryCriteria>(request.QueryCriteria);

        var inventoriesResponse = await _inventoryService
            .GetAllAsync(mappedCriteria, ct);

        if (!inventoriesResponse.Succeeded || inventoriesResponse.Data is null)
            return new Response<PagedResult<InventoryResponseDto>>(
                "Failed to retrieve inventories.",
                inventoriesResponse.Errors);

        var mappedItems = _mapper.Map<List<InventoryResponseDto>>(
            inventoriesResponse.Data.Items);

        var pagedResponse = new PagedResult<InventoryResponseDto>
        {
            Items = mappedItems,
            TotalCount = inventoriesResponse.Data.TotalCount,
            PageNumber = inventoriesResponse.Data.PageNumber,
            PageSize = inventoriesResponse.Data.PageSize
        };

        return new Response<PagedResult<InventoryResponseDto>>(pagedResponse);
    }
}
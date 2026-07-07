using AutoMapper;
using FluentValidation;
using MediatR;
using ShoppingSystem.Microservice.Application.Common;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Inventory.Queries.GetLowStock;

public sealed class GetLowStockInventoryQueryHandler
    : IRequestHandler<GetLowStockInventoryQuery,
        Response<PagedResult<InventoryResponseDto>>>
{
    private readonly IInventoryService _inventoryService;
    private readonly IMapper _mapper;
    private readonly IValidator<GetLowStockInventoryQuery> _validator;

    public GetLowStockInventoryQueryHandler(
        IInventoryService inventoryService,
        IMapper mapper,
        IValidator<GetLowStockInventoryQuery> validator)
    {
        _inventoryService = inventoryService;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<Response<PagedResult<InventoryResponseDto>>> Handle(
        GetLowStockInventoryQuery request,
        CancellationToken ct)
    {
        var validationResult = await _validator.ValidateAsync(request, ct);

        if (!validationResult.IsValid)
        {
            return new Response<PagedResult<InventoryResponseDto>>
            {
                Succeeded = false,
                Message = "Validation failed.",
                Errors = validationResult.Errors
                    .Select(x => x.ErrorMessage)
                    .ToList()
            };
        }

        var mappedCriteria = _mapper.Map<QueryCriteria>(request.QueryCriteriaRequestDto);

        var result = await _inventoryService.GetLowStockInventoryAsync(
            mappedCriteria,
            request.lowStockInventories,
            ct);

        if (!result.Succeeded || result.Data is null)
        {
            return new Response<PagedResult<InventoryResponseDto>>
            {
                Succeeded = false,
                Message = result.Message ?? "No low stock inventories found.",
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
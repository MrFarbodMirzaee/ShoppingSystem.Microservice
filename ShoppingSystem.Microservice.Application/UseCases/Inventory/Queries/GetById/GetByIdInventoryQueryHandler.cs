using AutoMapper;
using FluentValidation;
using MediatR;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Inventory.Queries.GetById;

public class GetByIdInventoryQueryHandler
    : IRequestHandler<GetByIdInventoryQuery, 
    Response<InventoryResponseDto>>
{
    private readonly IInventoryService _inventoryService;
    private readonly IMapper _mapper;
    private readonly IValidator<GetByIdInventoryQuery> _validator;

    public GetByIdInventoryQueryHandler
    (
        IInventoryService inventoryService,
        IMapper mapper,
        IValidator<GetByIdInventoryQuery> validator
    )
    {
        _inventoryService = inventoryService;
        _mapper = mapper;
        _validator = validator;
    }


    public async Task<Response<InventoryResponseDto>> Handle
    (
        GetByIdInventoryQuery request,
        CancellationToken ct
    )
    {
        
        var validationResult = await _validator
            .ValidateAsync(request, ct);

        if (!validationResult.IsValid)
        {
            return new Response<InventoryResponseDto>
            {
                Succeeded = false,
                Message = "Validation failed.",
                Errors = validationResult.Errors
                    .Select(x => x.ErrorMessage)
                    .ToList(),
                Data = null
            };
        }
        
        var inventoryResponse = await _inventoryService
            .GetByIdAsync(request.InventoryId, ct);


        if (!inventoryResponse.Succeeded || inventoryResponse.Data is null)
            return new Response<InventoryResponseDto>(
                "Inventory not found."
            );


        var inventory = _mapper
            .Map<InventoryResponseDto>(
                inventoryResponse.Data
            );


        return new Response
        <InventoryResponseDto>(
            inventory
        );
    }
}
using AutoMapper;
using MediatR;
using ShoppingSystem.Microservice.Application.Common;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Address.Queries.GetAll;

public sealed class GetAllAddressQueryHandler
    : IRequestHandler<GetAllAddressQuery, Response<PagedResult<AddressResponseDto>>>
{
    private readonly IAddressService _addressService;
    private readonly IMapper _mapper;

    public GetAllAddressQueryHandler(
        IAddressService addressService,
        IMapper mapper)
    {
        _addressService = addressService;
        _mapper = mapper;
    }

    public async Task<Response<PagedResult<AddressResponseDto>>> Handle(
        GetAllAddressQuery request,
        CancellationToken ct)
    {
        
        var mappedCriteria = _mapper.Map<QueryCriteria>(request.Criteria);
        
        var result = await _addressService.GetAllAsync(mappedCriteria, ct);

        if (!result.Succeeded || result.Data is null)
            return new Response<PagedResult<AddressResponseDto>>(
                "It couldn't retrieve addresses.",
                result.Errors);

        var mappedItems = _mapper.Map<List<AddressResponseDto>>(result.Data.Items);

        var pagedResponse = new PagedResult<AddressResponseDto>
        {
            Items = mappedItems,
            TotalCount = result.Data.TotalCount,
            PageNumber = result.Data.PageNumber,
            PageSize = result.Data.PageSize
        };

        return new Response<PagedResult<AddressResponseDto>>(pagedResponse);
    }
}
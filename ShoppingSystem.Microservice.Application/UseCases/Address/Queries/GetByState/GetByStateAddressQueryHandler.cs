using AutoMapper;
using FluentValidation;
using MediatR;
using ShoppingSystem.Microservice.Application.Common;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Address.Queries.GetByState;

public sealed class GetByStateAddressQueryHandler
    : IRequestHandler<GetByStateAddressQuery,
        Response<PagedResult<AddressResponseDto>>>
{
    private readonly IAddressService _addressService;
    private readonly IMapper _mapper;
    private readonly IValidator<GetByStateAddressQuery> _validator;

    public GetByStateAddressQueryHandler(
        IAddressService addressService,
        IMapper mapper,
        IValidator<GetByStateAddressQuery> validator)
    {
        _addressService = addressService;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<Response<PagedResult<AddressResponseDto>>> Handle(
        GetByStateAddressQuery request,
        CancellationToken ct)
    {
        var validationResult = await _validator.ValidateAsync(request, ct);

        if (!validationResult.IsValid)
        {
            return new Response<PagedResult<AddressResponseDto>>
            {
                Succeeded = false,
                Message = "Validation failed.",
                Errors = validationResult.Errors
                    .Select(x => x.ErrorMessage)
                    .ToList()
            };
        }

        var mappedCriteria = _mapper.Map<QueryCriteria>(request.QueryCriteriaRequestDto);

        var result = await _addressService.GetByStateAsync(
            mappedCriteria,
            request.State,
            ct);

        if (!result.Succeeded || result.Data is null)
        {
            return new Response<PagedResult<AddressResponseDto>>
            {
                Succeeded = false,
                Message = result.Message ?? "No addresses found for this state.",
                Errors = result.Errors
            };
        }

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
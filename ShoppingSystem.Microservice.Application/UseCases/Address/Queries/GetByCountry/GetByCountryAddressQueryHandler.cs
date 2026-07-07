using AutoMapper;
using FluentValidation;
using MediatR;
using ShoppingSystem.Microservice.Application.Common;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Address.Queries.GetByCountry;

public sealed class GetByCountryAddressQueryHandler
    : IRequestHandler<GetByCountryAddressQuery, Response<PagedResult<AddressResponseDto>>>
{
    private readonly IAddressService _addressService;
    private readonly IMapper _mapper;
    private readonly IValidator<GetByCountryAddressQuery> _validator;

    public GetByCountryAddressQueryHandler(
        IAddressService addressService,
        IMapper mapper,
        IValidator<GetByCountryAddressQuery> validator)
    {
        _addressService = addressService;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<Response<PagedResult<AddressResponseDto>>> Handle(
        GetByCountryAddressQuery request,
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

        var result = await _addressService.GetByCountryAsync(
            mappedCriteria,
            request.Country,
            ct);

        if (!result.Succeeded || result.Data is null)
            return new Response<PagedResult<AddressResponseDto>>
            {
                Succeeded = false,
                Message = "Unable to retrieve addresses.",
                Errors = result.Errors
            };

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
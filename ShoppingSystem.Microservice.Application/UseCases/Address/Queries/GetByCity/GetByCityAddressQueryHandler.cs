using AutoMapper;
using FluentValidation;
using MediatR;
using ShoppingSystem.Microservice.Application.Common;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Address.Queries.GetByCity;

public sealed class GetByCityAddressQueryHandler
    : IRequestHandler<GetByCityAddressQuery, Response<PagedResult<AddressResponseDto>>>
{
    private readonly IAddressService _addressService;
    private readonly IMapper _mapper;
    private readonly IValidator<GetByCityAddressQuery> _validator;

    public GetByCityAddressQueryHandler
    (
        IAddressService addressService,
        IMapper mapper,
        IValidator<GetByCityAddressQuery> validator
    )
    {
        _addressService = addressService;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<Response<PagedResult<AddressResponseDto>>> Handle(
        GetByCityAddressQuery request,
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

        var mappedCriteria = _mapper.Map<QueryCriteria>(request.Criteria);

        var result = await _addressService.GetByCityAsync(
            request.City,
            mappedCriteria,
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

        return new Response
        <PagedResult<AddressResponseDto>>(pagedResponse);
    }
}
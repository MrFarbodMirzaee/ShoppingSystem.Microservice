using AutoMapper;
using FluentValidation;
using MediatR;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Address.Queries.GetById;

public sealed class GetByIdAddressQueryHandler
    : IRequestHandler<GetByIdAddressQuery,
        Response<AddressResponseDto>>
{
    private readonly IAddressService _addressService;
    private readonly IMapper _mapper;
    private readonly IValidator<GetByIdAddressQuery> _validator;
    
    public GetByIdAddressQueryHandler
    (
        IAddressService addressService,
        IMapper mapper,
        IValidator<GetByIdAddressQuery> validator
    )
    {
        _addressService = addressService;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<Response<AddressResponseDto>> Handle
    (
        GetByIdAddressQuery request,
        CancellationToken ct
    )
    {
        
        var validationResult = await _validator
            .ValidateAsync(request, ct);

        if (!validationResult.IsValid)
        {
            return new Response<AddressResponseDto>
            {
                Succeeded = false,
                Message = "Validation failed.",
                Errors = validationResult.Errors
                    .Select(x => x.ErrorMessage)
                    .ToList(),
                Data = null
            };
        }
        
        var addressResponse = await _addressService
            .GetByIdAsync(request.Id, ct);

        if (!addressResponse.Succeeded || addressResponse.Data is null)
            return new Response<AddressResponseDto>
                ("Address not found.");

        var address = _mapper
            .Map<AddressResponseDto>(addressResponse.Data);

        return new Response
        <AddressResponseDto>(address);
    }
}
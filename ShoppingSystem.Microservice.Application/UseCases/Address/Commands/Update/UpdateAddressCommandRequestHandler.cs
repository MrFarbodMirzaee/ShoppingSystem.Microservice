using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Application.UseCases.Address.Commands.DeleteById;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Address.Commands.Update;

public class UpdateAddressCommandRequestHandler
    : IRequestHandler<UpdateAddressCommandRequest, Response<bool>>
{
    private readonly IAddressService _addressRepository;
    private readonly IUnitOfWorkBase _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<UpdateAddressCommandRequest> _validator;
    private readonly ILogger<DeleteByIdAddressCommandRequestHandler> _logger;
    
    public UpdateAddressCommandRequestHandler
    (
        IAddressService addressRepository,
        IUnitOfWorkBase unitOfWork,
        IMapper mapper,
        IValidator<UpdateAddressCommandRequest> validator,
        ILogger<DeleteByIdAddressCommandRequestHandler> logger
    )
    {
        _addressRepository = addressRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Response<bool>> Handle(
        UpdateAddressCommandRequest request,
        CancellationToken ct)
    {
        _logger.LogInformation(
            "Starting address update. AddressId: {AddressId}.",
            request.Id);

        var validationResult = await _validator
            .ValidateAsync(request, ct);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning(
                "Address update validation failed for AddressId {AddressId}. Errors: {Errors}",
                request.Id,
                validationResult.Errors.Select(x => x.ErrorMessage));

            return new Response<bool>
            {
                Succeeded = false,
                Message = "Validation failed.",
                Errors = validationResult.Errors
                    .Select(x => x.ErrorMessage)
                    .ToList(),
                Data = false
            };
        }

        var addressResponse = await _addressRepository
            .GetByIdAsync(request.Id, ct);

        if (addressResponse is null)
        {
            _logger.LogWarning(
                "Address update failed. AddressId {AddressId} was not found.",
                request.Id);

            return new Response<bool>(
                false,
                "Address not found.");
        }

        var address = addressResponse.Data;

        _mapper.Map(request, address);

        await _addressRepository
            .UpdateAsync(address, ct);

        await _unitOfWork
            .SaveAsync(ct);

        _logger.LogInformation(
            "Address updated successfully. AddressId: {AddressId}.",
            request.Id);

        return new Response<bool>(true);
    }
}
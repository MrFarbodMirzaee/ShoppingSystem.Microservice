using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Address.Commands.DeleteById;

public class DeleteByIdAddressCommandRequestHandler
    : IRequestHandler<DeleteByIdAddressCommandRequest, Response<bool>>
{
    private readonly IAddressService _addressRepository;
    private readonly IUnitOfWorkBase _unitOfWork;
    private readonly IValidator<DeleteByIdAddressCommandRequest> _validator;
    private readonly ILogger<DeleteByIdAddressCommandRequestHandler> _logger;

    public DeleteByIdAddressCommandRequestHandler(
        IAddressService addressRepository,
        IUnitOfWorkBase unitOfWork,
        IValidator<DeleteByIdAddressCommandRequest> validator,
        ILogger<DeleteByIdAddressCommandRequestHandler> logger
        )
    {
        _addressRepository = addressRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Response<bool>> Handle(
        DeleteByIdAddressCommandRequest request,
        CancellationToken ct)
    {
        _logger.LogInformation(
            "Starting address deletion. AddressId: {AddressId}.",
            request.Id);

        var validationResult = await _validator
            .ValidateAsync(request, ct);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning(
                "Address deletion validation failed for AddressId {AddressId}. Errors: {Errors}",
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

        var response = await _addressRepository
            .DeleteByIdAsync(request.Id, ct);

        if (!response.Data)
        {
            _logger.LogWarning(
                "Address deletion failed. AddressId {AddressId} was not found.",
                request.Id);

            return new Response<bool>(
                false,
                "Address not found.");
        }

        await _unitOfWork.SaveAsync(ct);

        _logger.LogInformation(
            "Address deleted successfully. AddressId: {AddressId}.",
            request.Id);

        return new Response<bool>(true);
    }
}
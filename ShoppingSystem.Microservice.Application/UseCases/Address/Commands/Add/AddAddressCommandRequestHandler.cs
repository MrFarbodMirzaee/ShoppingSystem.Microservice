using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Address.Commands.Add;

public class AddAddressCommandRequestHandler
: IRequestHandler<AddAddressRequestCommand , Response<bool>>
{
    private readonly IAddressService _addressRepository;
    private readonly IUnitOfWorkBase _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<AddAddressRequestCommand> _validator;
    private readonly IUserQueryService _userQueryService;
    private readonly ILogger<AddAddressCommandRequestHandler> _logger;

    public AddAddressCommandRequestHandler
    (IAddressService addressRepository,
    IUnitOfWorkBase unitOfWork,
    IMapper mapper,
    IValidator<AddAddressRequestCommand> validator,
    IUserQueryService userQueryService,
    ILogger<AddAddressCommandRequestHandler> logger
    
    )
    {
        _addressRepository = addressRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validator = validator;
        _userQueryService = userQueryService;
        _logger = logger;
    }

    public async Task<Response<bool>> Handle
    (AddAddressRequestCommand request,
    CancellationToken ct)
    {
        _logger.LogInformation(
            "Starting address creation for UserId {UserId}.",
            request.UserId);

        var validationResult = await _validator.ValidateAsync(request, ct);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning(
                "Address creation validation failed for UserId {UserId}. Errors: {Errors}",
                request.UserId,
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

        var userExists = await _userQueryService
            .UserExistsAsync(request.UserId, ct);

        if (!userExists)
        {
            _logger.LogWarning(
                "Address creation failed. User {UserId} does not exist.",
                request.UserId);

            return new Response<bool>(
                false,
                "User does not exist.");
        }

        var exists = await _addressRepository.ExistsAsync(
            request.Street,
            request.City,
            request.State,
            request.Country,
            request.PostalCode,
            ct);

        if (exists.Data)
        {
            _logger.LogWarning(
                "Duplicate address detected for UserId {UserId}. Address: {Street}, {City}, {Country}.",
                request.UserId,
                request.Street,
                request.City,
                request.Country);

            return new Response<bool>(
                false,
                "Address already exists.");
        }

        var address = _mapper.Map<Domain.Entities.Address>(request);

        _addressRepository.Add(address, ct);

        await _unitOfWork.SaveAsync(ct);

        _logger.LogInformation(
            "Address created successfully for UserId {UserId}. AddressId {AddressId}.",
            request.UserId,
            address.Id);

        return new Response<bool>(true);
    }
}
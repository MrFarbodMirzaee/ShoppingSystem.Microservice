using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.ValueObjects;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Payment.Commands.Add;

public class AddPaymentCommandHandler
    : IRequestHandler<AddPaymentCommand, Response<bool>>
{
    private readonly IPaymentService _paymentService;
    private readonly IUnitOfWorkBase _unitOfWork;
    private readonly IValidator<AddPaymentCommand> _validator;
    private readonly IUserQueryService _userQueryService;
    private readonly ILogger<AddPaymentCommandHandler> _logger;

    public AddPaymentCommandHandler
    (
        IPaymentService paymentService,
        IUnitOfWorkBase unitOfWork,
        IValidator<AddPaymentCommand> validator, 
        IUserQueryService userQueryService,
        ILogger<AddPaymentCommandHandler> logger
    )
    {
        _paymentService = paymentService;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _userQueryService = userQueryService;
        _logger = logger;
    }

    public async Task<Response<bool>> Handle(
    AddPaymentCommand request,
    CancellationToken ct)
{
    _logger.LogInformation(
        "Starting payment creation. OrderId: {OrderId}, UserId: {UserId}, Amount: {Amount}, Currency: {Currency}.",
        request.OrderId,
        request.UserId,
        request.Amount,
        request.Currency);

    var validationResult = await _validator
        .ValidateAsync(request, ct);

    if (!validationResult.IsValid)
    {
        _logger.LogWarning(
            "Payment creation validation failed for OrderId {OrderId}. Errors: {Errors}",
            request.OrderId,
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
            "Payment creation failed. UserId {UserId} does not exist.",
            request.UserId);

        return new Response<bool>(
            false,
            "User does not exist.");
    }

    var exists = await _paymentService
        .ExistsByOrderIdAsync(request.OrderId, ct);

    if (exists.Data)
    {
        _logger.LogWarning(
            "Payment creation failed. Payment already exists for OrderId {OrderId}.",
            request.OrderId);

        return new Response<bool>(
            false,
            "Payment already exists for this order.");
    }

    var amount = Money.Create(
        request.Amount,
        request.Currency);

    var payment = Domain.Aggregates.Payment.Payment.Create(
        request.OrderId,
        amount);

    _paymentService.Add(payment, ct);

    await _unitOfWork.SaveAsync(ct);

    _logger.LogInformation(
        "Payment created successfully. PaymentId: {PaymentId}, OrderId: {OrderId}, Currency: {Currency}.",
        payment.Id,
        payment.OrderId,
        payment.Amount.Currency);

    return new Response<bool>(true);
}
}
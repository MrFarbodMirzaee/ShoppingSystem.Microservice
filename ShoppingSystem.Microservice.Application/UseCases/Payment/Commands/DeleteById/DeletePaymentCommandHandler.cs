using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Payment.Commands.DeleteById;

public class DeletePaymentCommandHandler
    : IRequestHandler<DeleteByIdPaymentCommand, Response<bool>>
{
    private readonly IPaymentService _paymentService;
    private readonly IUnitOfWorkBase _unitOfWork;
    private readonly IValidator<DeleteByIdPaymentCommand> _validator;
    private readonly ILogger<DeletePaymentCommandHandler> _logger;

    public DeletePaymentCommandHandler
    (
        IPaymentService paymentService,
        IUnitOfWorkBase unitOfWork,
        IValidator<DeleteByIdPaymentCommand> validator,
        ILogger<DeletePaymentCommandHandler> logger
    )
    {
        _paymentService = paymentService;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Response<bool>> Handle(
        DeleteByIdPaymentCommand request,
        CancellationToken ct)
    {
        _logger.LogInformation(
            "Starting payment deletion. PaymentId: {PaymentId}.",
            request.PaymentId);

        var validationResult = await _validator
            .ValidateAsync(request, ct);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning(
                "Payment deletion validation failed for PaymentId {PaymentId}. Errors: {Errors}",
                request.PaymentId,
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

        var paymentResponse = await _paymentService
            .GetByIdAsync(request.PaymentId, ct);

        if (!paymentResponse.Succeeded || paymentResponse.Data is null)
        {
            _logger.LogWarning(
                "Payment deletion failed. PaymentId {PaymentId} was not found.",
                request.PaymentId);

            return new Response<bool>(
                false,
                "Payment not found.");
        }

        var payment = paymentResponse.Data;

        await _paymentService.DeleteByIdAsync(
            payment.Id,
            ct);

        await _unitOfWork.SaveAsync(ct);

        _logger.LogInformation(
            "Payment deleted successfully. PaymentId: {PaymentId}, OrderId: {OrderId}, Currency: {Currency}.",
            payment.Id,
            payment.OrderId,
            payment.Amount.Currency);

        return new Response<bool>(true);
    }
}
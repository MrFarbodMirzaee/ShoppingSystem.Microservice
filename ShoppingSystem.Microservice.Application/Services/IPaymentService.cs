using ShoppingSystem.Microservice.Application.Common;
using ShoppingSystem.Microservice.Domain.Aggregates.Payment;
using ShoppingSystem.Microservice.Domain.Enums;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.Services;

/// <summary>
/// ToDo:add use cases interfaces
/// </summary>
public interface IPaymentService 
: IRepository<Payment>
{
    #region Gets
    Task<Response<Payment>> GetByOrderIdAsync(
        Guid orderId,
        CancellationToken ct);

    Task<Response<Payment>> GetByTransactionIdAsync(
        string transactionId,
        CancellationToken ct);

    Task<Response<PagedResult<Payment>>> GetByStatusAsync(
        QueryCriteria queryCriteria,
        PaymentStatus status,
        CancellationToken ct);
    Task<Response<PagedResult<Payment>>> GetSuccessfulPaymentsAsync(
        QueryCriteria queryCriteria,
        CancellationToken ct);

    Task<Response<PagedResult<Payment>>> GetFailedPaymentsAsync(
        QueryCriteria queryCriteria,
        CancellationToken ct);

    Task<Response<PagedResult<Payment>>> GetRefundedPaymentsAsync(
        QueryCriteria queryCriteria,
        CancellationToken ct);
        #endregion

    Task<Response<bool>> ExistsByOrderIdAsync(
        Guid orderId,
        CancellationToken ct);

}
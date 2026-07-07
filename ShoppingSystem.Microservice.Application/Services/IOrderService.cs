using ShoppingSystem.Microservice.Application.Common;
using ShoppingSystem.Microservice.Domain.Aggregates.Order;
using ShoppingSystem.Microservice.Domain.Enums;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.Services;

/// <summary>
/// ToDo:add use cases interfaces
/// </summary>
public interface IOrderService 
: IRepository<Order>
{
    #region Gets
    Task<Response<Order>> GetByOrderNumberAsync(
        string orderNumber,
        CancellationToken ct);

    Task<Response<PagedResult<Order>>> GetByUserIdAsync(
        QueryCriteria queryCriteria,
        Guid userId,
        CancellationToken ct);

    Task<Response<PagedResult<Order>>> GetByStatusAsync(
        QueryCriteria queryCriteria,
        OrderStatus status,
        CancellationToken ct);
    #endregion

    Task<Response<bool>> ExistsByOrderNumberAsync(
        string orderNumber,
        CancellationToken ct);
}
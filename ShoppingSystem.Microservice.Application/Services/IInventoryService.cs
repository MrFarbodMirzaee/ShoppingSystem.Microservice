using ShoppingSystem.Microservice.Application.Common;
using ShoppingSystem.Microservice.Domain.Aggregates.Inventory;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.Services;

public interface IInventoryService 
: IRepository<Inventory>
{
    #region Gets
    Task<Response<Inventory>> GetByProductIdAsync(
        Guid productId,
        CancellationToken ct);
    
    Task<Response<PagedResult<Inventory>>> GetAvailableInventoryAsync(
        QueryCriteria queryCriteria,
        CancellationToken ct);

    Task<Response<PagedResult<Inventory>>> GetOutOfStockInventoryAsync(
        QueryCriteria queryCriteria,
        CancellationToken ct);

    Task<Response<PagedResult<Inventory>>> GetLowStockInventoryAsync(
        QueryCriteria queryCriteria,
        int lowStockInventories,
        CancellationToken ct);
        #endregion

    Task<Response<bool>> ExistsByProductIdAsync(
        Guid productId,
        CancellationToken ct);
    
    Task<Response<bool>> DecreaseStockAsync(
        Guid productId,
        byte quantity,
        CancellationToken ct);

}
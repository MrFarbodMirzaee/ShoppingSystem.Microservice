using ShoppingSystem.Microservice.Domain.Aggregates.Cart;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.Services;

/// <summary>
/// ToDo:add use cases interfaces
/// </summary>
public interface ICartService : IRepository<Cart>
{
    Task<Response<Cart>> GetByCustomerIdAsync(
        Guid customerId,
        CancellationToken ct);

    Task<Response<bool>> ExistsByCustomerIdAsync(
        Guid customerId,
        CancellationToken ct);

    Task<Response<bool>> HasItemsAsync(
        Guid customerId,
        CancellationToken ct);
}
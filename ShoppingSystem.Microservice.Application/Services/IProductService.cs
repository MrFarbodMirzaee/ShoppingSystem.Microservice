using ShoppingSystem.Microservice.Application.Common;
using ShoppingSystem.Microservice.Domain.Aggregates.Product;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.Services;

public interface IProductService : IRepository<Product>
{
    Task<Response<Product>> GetByNameAsync(
        string name,
        CancellationToken ct);

    Task<Response<PagedResult<Product>>> GetByCategoryAsync(
        Guid categoryId,
        QueryCriteria queryCriteria,
        CancellationToken ct);

    Task<Response<PagedResult<Product>>> GetAvailableProductsAsync(
        QueryCriteria queryCriteria,
        CancellationToken ct);
    
    Task<Response<bool>> ExistsByNameAsync(
        string name,
        CancellationToken ct);
}
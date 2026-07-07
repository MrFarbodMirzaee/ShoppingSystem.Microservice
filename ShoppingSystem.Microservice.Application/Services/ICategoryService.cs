using ShoppingSystem.Microservice.Application.Common;
using ShoppingSystem.Microservice.Domain.Aggregates.Product;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.Services;

/// <summary>
/// ToDo:add use cases interfaces
/// </summary>
public interface ICategoryService : IRepository<Category>
{
    Task<Response<Category>> GetByNameAsync(
        string name,
        CancellationToken ct);

    Task<Response<PagedResult<Category>>> GetActiveCategoriesAsync
    (QueryCriteria queryCriteria, CancellationToken ct);
    
    Task<Response<bool>> ExistsByNameAsync(
        string name,
        CancellationToken ct);

        
}
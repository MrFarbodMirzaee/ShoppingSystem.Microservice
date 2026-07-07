using System.Linq.Expressions;
using ShoppingSystem.Microservice.Application.Common;
using ShoppingSystem.Microservice.Domain.Common;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.Services;

public interface IRepository<TEntity> 
where TEntity : BaseEntity
{
    Task<Response<PagedResult<TEntity>>> GetAllAsync(
        QueryCriteria criteria,
        CancellationToken ct);
    
    Task<Response<TEntity>> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Response<List<TEntity>>> FindByCondition(Expression<Func<TEntity, bool>> expression, CancellationToken ct);
    Task<Response<bool>> AddAsync(TEntity entity, CancellationToken ct);
    void Add(TEntity entity, CancellationToken ct);
    Task<Response<bool>> UpdateAsync(TEntity entity, CancellationToken ct);
    Task<Response<bool>> DeleteAsync(TEntity entity, CancellationToken ct);
    Task<Response<bool>> DeleteByIdAsync(Guid id, CancellationToken ct);
}
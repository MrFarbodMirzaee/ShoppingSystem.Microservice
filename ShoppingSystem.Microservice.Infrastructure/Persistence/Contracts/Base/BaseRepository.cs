using System.Linq.Expressions;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using ShoppingSystem.Microservice.Application.Common;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Common;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace Infrastructure.Persistence.Contracts.Base;

public class BaseRepository<TEntity> :
    IRepository<TEntity> where TEntity : BaseEntity
{
    internal AppDbContext _appDbContext { get; }
    protected DbSet<TEntity> DbSet { get; set; }

    internal BaseRepository(AppDbContext context)
    {
        _appDbContext = context ?? 
                        throw new ArgumentNullException(nameof(context));
        
        DbSet = context.Set<TEntity>();
    }

    public async Task<Response<PagedResult<TEntity>>> GetAllAsync(
        QueryCriteria criteria,
        CancellationToken ct)
    {
        var query = DbSet
            .AsNoTracking()
            .OrderBy(x => x.Id);

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .Skip(criteria.Skip)
            .Take(criteria.PageSize)
            .ToListAsync(ct);

        var result = new PagedResult<TEntity>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = criteria.PageNumber,
            PageSize = criteria.PageSize
        };

        return new Response
        <PagedResult<TEntity>>(result);
    }

    public async Task<Response<TEntity>> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var entity = await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync
                (prop => prop.Id == id, ct);


        return new Response<TEntity>(entity);
    }

    public async Task<Response<List<TEntity>>> FindByCondition(
        Expression<Func<TEntity, bool>> expression,
        CancellationToken ct)
    {
        var entities = await DbSet
            .AsNoTracking()
            .Where(expression)
            .ToListAsync(ct);

        return new
            Response<List<TEntity>>(entities);
    }

    public async Task<Response<bool>> AddAsync(TEntity entity, CancellationToken ct)
    {
        if (entity is null)
            throw new
                ArgumentNullException(nameof(entity));

        await DbSet
            .AddAsync(entity, ct);

        var affectedRows = await _appDbContext
            .SaveChangesAsync(ct);

        return new Response<bool>
            (affectedRows > 0);
    }

    public void Add(TEntity entity, CancellationToken ct)
    {
        if (entity is null)
            throw new
                ArgumentNullException(nameof(entity));

        DbSet
            .Add(entity);
    }

    public async Task<Response<bool>> UpdateAsync(TEntity entity, CancellationToken ct)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        DbSet.Update(entity);

        var affectedRows = await _appDbContext
            .SaveChangesAsync(ct);

        return new Response<bool>
            (affectedRows > 0);
    }

    public async Task<Response<bool>> DeleteAsync(TEntity entity, CancellationToken ct)
    {
        if (entity is null)
            throw new
                ArgumentNullException(nameof(entity));

        entity.Delete();
        
        DbSet.Update(entity);

        var affectedRows = await _appDbContext
            .SaveChangesAsync(ct);

        return new Response<bool>
            (affectedRows > 0);
    }

    public async Task<Response<bool>> DeleteByIdAsync(Guid id, CancellationToken ct)
    {
        var response = await GetByIdAsync(id, ct);

        var entity = response.Data;

        if (entity is null)
            return new Response<bool>
                (false);

        entity.Delete();

        DbSet.Update(entity);

        var affectedRows = await _appDbContext
            .SaveChangesAsync(ct);

        return new Response<bool>
            (affectedRows > 0);
    }
}
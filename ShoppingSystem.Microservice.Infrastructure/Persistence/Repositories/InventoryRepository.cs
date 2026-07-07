using Infrastructure.Contexts;
using Infrastructure.Persistence.Contracts;
using Microsoft.EntityFrameworkCore;
using ShoppingSystem.Microservice.Application.Attributes;
using ShoppingSystem.Microservice.Application.Common;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Aggregates.Inventory;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace Infrastructure.Persistence.Repositories;

[ScopedService]
public class InventoryRepository
    : Repository<Inventory>, IInventoryService
{
    public InventoryRepository(AppDbContext context)
        : base(context)
    {
    }

    public async Task<Response<Inventory>> GetByProductIdAsync(
        Guid productId,
        CancellationToken ct)
    {
        var inventory = await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.ProductId == productId,
                ct);

        return new Response
        <Inventory>(inventory);
    }

    public async Task<Response<bool>> ExistsByProductIdAsync(
        Guid productId,
        CancellationToken ct)
    {
        var exists = await DbSet
            .Where(x => x.ProductId == productId)
            .AnyAsync(ct);

        return new Response
        <bool>(exists);
    }

    public async Task<Response<bool>> DecreaseStockAsync(Guid productId, byte quantity, CancellationToken ct)
    {
        var result = await 
            GetByProductIdAsync(productId, ct);

        if (!result.Succeeded || result.Data is null)
        {
            return new Response<bool>
            {
                Succeeded = false,
                Message = "Inventory not found.",
                Data = false
            };
        }

        var inventory = result.Data;

        inventory.DecreaseStock(quantity);

        await UpdateAsync(inventory, ct);

        return new Response<bool>
        {
            Succeeded = true,
            Message = "Stock decreased successfully.",
            Data = true
        };
    }

    public async Task<Response<PagedResult<Inventory>>> GetAvailableInventoryAsync(
        QueryCriteria criteria,
        CancellationToken ct)
    {
        var query = DbSet
            .AsNoTracking()
            .Where(x => x.Quantity > 0)
            .OrderBy(x => x.Id);

        var totalCount = await query.CountAsync(ct);

        var inventories = await query
            .Skip(criteria.Skip)
            .Take(criteria.PageSize)
            .ToListAsync(ct);

        var pagedResult = new PagedResult<Inventory>
        {
            Items = inventories,
            TotalCount = totalCount,
            PageNumber = criteria.PageNumber,
            PageSize = criteria.PageSize
        };

        return new Response<PagedResult<Inventory>>(pagedResult);
    }

    public async Task<Response<PagedResult<Inventory>>> GetOutOfStockInventoryAsync(
        QueryCriteria criteria,
        CancellationToken ct)
    {
        var query = DbSet
            .AsNoTracking()
            .Where(x => x.Quantity == 0)
            .OrderBy(x => x.Id);

        var totalCount = await query.CountAsync(ct);

        var inventories = await query
            .Skip(criteria.Skip)
            .Take(criteria.PageSize)
            .ToListAsync(ct);

        var pagedResult = new PagedResult<Inventory>
        {
            Items = inventories,
            TotalCount = totalCount,
            PageNumber = criteria.PageNumber,
            PageSize = criteria.PageSize
        };

        return new Response
        <PagedResult<Inventory>>(pagedResult);
    }

    public async Task<Response<PagedResult<Inventory>>> GetLowStockInventoryAsync
    (QueryCriteria criteria,
    int lowStockInventories,
    CancellationToken ct)
    {
        var query = DbSet
            .AsNoTracking()
            .Where(x => x.Quantity <= lowStockInventories)
            .OrderBy(x => x.Id);

        var totalCount = await query.CountAsync(ct);

        var inventories = await query
            .Skip(criteria.Skip)
            .Take(criteria.PageSize)
            .ToListAsync(ct);

        var pagedResult = new PagedResult<Inventory>
        {
            Items = inventories,
            TotalCount = totalCount,
            PageNumber = criteria.PageNumber,
            PageSize = criteria.PageSize
        };

        return new Response
        <PagedResult<Inventory>>(pagedResult);
    }

}
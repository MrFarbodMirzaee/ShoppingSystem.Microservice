using Infrastructure.Contexts;
using Infrastructure.Persistence.Contracts;
using Microsoft.EntityFrameworkCore;
using ShoppingSystem.Microservice.Application.Attributes;
using ShoppingSystem.Microservice.Application.Common;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Aggregates.Order;
using ShoppingSystem.Microservice.Domain.Enums;
using ShoppingSystem.Microservice.Domain.ValueObjects;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace Infrastructure.Persistence.Repositories;

[ScopedService]
public class OrderRepository
  : Repository<Order>, IOrderService
{
    public OrderRepository(AppDbContext context)
        : base(context)
    {
    }

    public async Task<Response<Order>> GetByOrderNumberAsync
        (string orderNumber, CancellationToken ct)
    {
        var order = await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.OrderNumber ==
                    OrderNumber.Create(orderNumber),
                ct);

        return new Response
        <Order>(order);
    }

    public async Task<Response<PagedResult<Order>>> GetByUserIdAsync
        (QueryCriteria criteria, Guid userId, CancellationToken ct)
    {
        var query = DbSet
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderBy(x => x.Id);

        var totalCount = await query.CountAsync(ct);

        var orders = await query
            .Skip(criteria.Skip)
            .Take(criteria.PageSize)
            .ToListAsync(ct);

        var pagedResult = new PagedResult<Order>
        {
            Items = orders,
            TotalCount = totalCount,
            PageNumber = criteria.PageNumber,
            PageSize = criteria.PageSize
        };

        return new Response
            <PagedResult<Order>>(pagedResult);
    }

    public async Task<Response<PagedResult<Order>>> GetByStatusAsync
    (QueryCriteria criteria, OrderStatus status, CancellationToken ct)
    {
        var query = DbSet
            .AsNoTracking()
            .Where(x => x.Status == status)
            .OrderBy(x => x.Id);

        var totalCount = await query.CountAsync(ct);

        var orders = await query
            .Skip(criteria.Skip)
            .Take(criteria.PageSize)
            .ToListAsync(ct);

        var pagedResult = new PagedResult<Order>
        {
            Items = orders,
            TotalCount = totalCount,
            PageNumber = criteria.PageNumber,
            PageSize = criteria.PageSize
        };

        return new Response
            <PagedResult<Order>>(pagedResult);
    }

    public async Task<Response<bool>> ExistsByOrderNumberAsync(string orderNumber, CancellationToken ct)
    {
        var exists = await DbSet
            .AnyAsync(x => x.OrderNumber 
                           == OrderNumber.Create(orderNumber), ct);

        return new Response
        <bool>(exists);
    }
}
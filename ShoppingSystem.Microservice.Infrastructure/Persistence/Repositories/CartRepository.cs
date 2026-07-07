using Infrastructure.Contexts;
using Infrastructure.Persistence.Contracts;
using Microsoft.EntityFrameworkCore;
using ShoppingSystem.Microservice.Application.Attributes;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Aggregates.Cart;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace Infrastructure.Persistence.Repositories;

[ScopedService]
public class CartRepository
: Repository<Cart>, ICartService
{
    public CartRepository(AppDbContext context)
        : base(context)
    {
    }

    public async Task<Response<Cart>> GetByCustomerIdAsync(
        Guid customerId,
        CancellationToken ct)
    {
        var cart = await DbSet
            .Include(x => x.Items)
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.CustomerId == customerId,
                ct);

        return new Response
        <Cart>(cart);
    }

    public async Task<Response<bool>> ExistsByCustomerIdAsync(
        Guid customerId,
        CancellationToken ct)
    {
        var exists = await DbSet
            .Where(x => x.CustomerId == customerId)
            .AnyAsync(ct);

        return new Response
        <bool>(exists);
    }

    public async Task<Response<bool>> HasItemsAsync(
        Guid customerId,
        CancellationToken ct)
    {
        var hasItems = await DbSet
            .Where(x => x.CustomerId == customerId)
            .AnyAsync(x => x.Items.Any(), ct);

        return new Response
        <bool>(hasItems);
    }
}
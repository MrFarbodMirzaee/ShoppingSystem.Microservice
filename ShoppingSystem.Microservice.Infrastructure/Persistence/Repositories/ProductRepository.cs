using Infrastructure.Contexts;
using Infrastructure.Persistence.Contracts;
using Microsoft.EntityFrameworkCore;
using ShoppingSystem.Microservice.Application.Attributes;
using ShoppingSystem.Microservice.Application.Common;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Aggregates.Product;
using ShoppingSystem.Microservice.Domain.ValueObjects;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace Infrastructure.Persistence.Repositories;

[ScopedService]
public class ProductRepository : 
Repository<Product> , IProductService
{
    public ProductRepository(AppDbContext context)
    : base(context)
    {
    }

    public async Task<Response<Product>> GetByNameAsync
    (string name, CancellationToken ct)
    {
        var product = await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Name.Value.ToLower() == name.ToLower(),
                ct);

        return new Response
        <Product>(product);
    }

    public async Task<Response<bool>> ExistsByNameAsync
        (string name, CancellationToken ct)
    {
        var exists = await DbSet
            .AnyAsync(
                x => x.Name == ProductName.Create(name),
                ct);

        return new Response
        <bool>(exists);
    }

    public async Task<Response<PagedResult<Product>>> GetByCategoryAsync(Guid categoryId,QueryCriteria criteria, CancellationToken ct)
    {
        var query = DbSet
            .AsNoTracking()
            .Where(x => x.CategoryId == categoryId)
            .OrderBy(x => x.Id);

        var totalCount = await query.CountAsync(ct);

        var products = await query
            .Skip(criteria.Skip)
            .Take(criteria.PageSize)
            .ToListAsync(ct);

        var pagedResult = new PagedResult<Product>
        {
            Items = products,
            TotalCount = totalCount,
            PageNumber = criteria.PageNumber,
            PageSize = criteria.PageSize
        };

        return new Response
            <PagedResult<Product>>(pagedResult);
    }

    public async Task<Response<PagedResult<Product>>> GetAvailableProductsAsync(
        QueryCriteria criteria,
        CancellationToken ct)
    {
        var query = DbSet
            .AsNoTracking()
            .Where(x => x.IsAvailable)
            .OrderBy(x => x.Id);

        var totalCount = await query.CountAsync(ct);

        var products = await query
            .Skip(criteria.Skip)
            .Take(criteria.PageSize)
            .ToListAsync(ct);

        var pagedResult = new PagedResult<Product>
        {
            Items = products,
            TotalCount = totalCount,
            PageNumber = criteria.PageNumber,
            PageSize = criteria.PageSize
        };

        return new Response
            <PagedResult<Product>>(pagedResult);
    }

    public async Task<Response<List<Product>>> SearchAsync(string? keyword, CancellationToken ct)
    {
        IQueryable<Product> query = DbSet.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            keyword = keyword.Trim();

            var productName = ProductName.Create(keyword);

            query = query.Where(x =>
                x.Name == productName ||
                x.Description == keyword);
        }

        var products = await query.ToListAsync(ct);

        return new Response
        <List<Product>>(products);
    }
}
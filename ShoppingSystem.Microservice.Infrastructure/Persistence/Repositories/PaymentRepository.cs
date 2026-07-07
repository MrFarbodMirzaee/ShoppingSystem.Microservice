using Infrastructure.Contexts;
using Infrastructure.Persistence.Contracts;
using Microsoft.EntityFrameworkCore;
using ShoppingSystem.Microservice.Application.Attributes;
using ShoppingSystem.Microservice.Application.Common;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Aggregates.Payment;
using ShoppingSystem.Microservice.Domain.Enums;
using ShoppingSystem.Microservice.Domain.ValueObjects;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace Infrastructure.Persistence.Repositories;

[ScopedService]
public class PaymentRepository
: Repository<Payment>, IPaymentService
{
    public PaymentRepository(AppDbContext context)
        : base(context)
    {
    }

    public async Task<Response<Payment>> GetByOrderIdAsync(
        Guid orderId,
        CancellationToken ct)
    {
        var payment = await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.OrderId == orderId,
                ct);

        return new Response
        <Payment>(payment);
    }

    public async Task<Response<Payment>> GetByTransactionIdAsync(
        string transactionId,
        CancellationToken ct)
    {
        var payment = await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.TransactionId 
                     == TransactionId.Create(transactionId),
                ct);

        return new Response
        <Payment>(payment);
    }

    public async Task<Response<PagedResult<Payment>>> GetByStatusAsync
    (QueryCriteria criteria, PaymentStatus status, CancellationToken ct)
    {
        var query = DbSet
            .AsNoTracking()
            .Where(x => x.Status == status)
            .OrderBy(x => x.Id);

        var totalCount = await query.CountAsync(ct);

        var payments = await query
            .Skip(criteria.Skip)
            .Take(criteria.PageSize)
            .ToListAsync(ct);

        var pagedResult = new PagedResult<Payment>
        {
            Items = payments,
            TotalCount = totalCount,
            PageNumber = criteria.PageNumber,
            PageSize = criteria.PageSize
        };

        return new Response
            <PagedResult<Payment>>(pagedResult);
    }

    public async Task<Response<bool>> ExistsByOrderIdAsync(
        Guid orderId,
        CancellationToken ct)
    {
        var exists = await DbSet
            .Where(x => x.OrderId == orderId)
            .AnyAsync(ct);

        return new Response
        <bool>(exists);
    }

    public async Task<Response<PagedResult<Payment>>> GetSuccessfulPaymentsAsync(
        QueryCriteria criteria,
        CancellationToken ct)
    {
        var query = DbSet
            .AsNoTracking()
            .Where(x => x.Status == PaymentStatus.Successful)
            .OrderBy(x => x.Id);

        var totalCount = await query.CountAsync(ct);

        var payments = await query
            .Skip(criteria.Skip)
            .Take(criteria.PageSize)
            .ToListAsync(ct);

        var pagedResult = new PagedResult<Payment>
        {
            Items = payments,
            TotalCount = totalCount,
            PageNumber = criteria.PageNumber,
            PageSize = criteria.PageSize
        };

        return new Response
            <PagedResult<Payment>>(pagedResult);
    }

    public async Task<Response<PagedResult<Payment>>> GetFailedPaymentsAsync(
        QueryCriteria criteria,
        CancellationToken ct)
    {
        var query = DbSet
            .AsNoTracking()
            .Where(x => x.Status == PaymentStatus.Failed)
            .OrderBy(x => x.Id);

        var totalCount = await query.CountAsync(ct);

        var payments = await query
            .Skip(criteria.Skip)
            .Take(criteria.PageSize)
            .ToListAsync(ct);

        var pagedResult = new PagedResult<Payment>
        {
            Items = payments,
            TotalCount = totalCount,
            PageNumber = criteria.PageNumber,
            PageSize = criteria.PageSize
        };

        return new Response
            <PagedResult<Payment>>(pagedResult);
    }

    public async Task<Response<PagedResult<Payment>>> GetRefundedPaymentsAsync(
        QueryCriteria criteria,
        CancellationToken ct)
    {
        var query = DbSet
            .AsNoTracking()
            .Where(x => x.Status == PaymentStatus.Refunded)
            .OrderBy(x => x.Id);

        var totalCount = await query.CountAsync(ct);

        var payments = await query
            .Skip(criteria.Skip)
            .Take(criteria.PageSize)
            .ToListAsync(ct);

        var pagedResult = new PagedResult<Payment>
        {
            Items = payments,
            TotalCount = totalCount,
            PageNumber = criteria.PageNumber,
            PageSize = criteria.PageSize
        };

        return new Response
            <PagedResult<Payment>>(pagedResult);
    }
}
using Infrastructure.Contexts;
using Infrastructure.Persistence.Contracts.Base;
using Infrastructure.Persistence.Options;
using ShoppingSystem.Microservice.Application.Attributes;

namespace Infrastructure.Persistence.Contracts;

[ScopedService]
public class UnitOfWork : 
    UnitOfWorkBase,IUnitOfWork
{
    public UnitOfWork(
        Option options,
        AppDbContext context)
        : base(options, context)
    {
    }
}
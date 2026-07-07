using Infrastructure.Contexts;
using Infrastructure.Persistence.Contracts.Base;
using ShoppingSystem.Microservice.Domain.Common;

namespace Infrastructure.Persistence.Contracts;

public class Repository<TEntity> :
    BaseRepository<TEntity> where TEntity : BaseEntity
{
    internal Repository(AppDbContext context) : base(context)
    {   
    }
}
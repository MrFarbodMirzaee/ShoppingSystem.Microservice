using Infrastructure.Persistence.Contracts.Base;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Common;

namespace Infrastructure.Persistence.Contracts;

public interface IUnitOfWork : IUnitOfWorkBase
{
    BaseRepository<T> GetRepository<T>() where T : BaseEntity;
}
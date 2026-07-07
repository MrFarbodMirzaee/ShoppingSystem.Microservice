namespace ShoppingSystem.Microservice.Application.Services;

public interface IUnitOfWorkBase : IDisposable
{
    bool IsDisposed { get; }

    void Save(CancellationToken ct);

    Task SaveAsync(CancellationToken ct);
    
}
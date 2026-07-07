using Microsoft.Extensions.DependencyInjection;

namespace ShoppingSystem.Microservice.Application.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class ScopedService : Attribute
{
    public ServiceLifetime Lifetime { get; }

    public ScopedService(
        ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        Lifetime = lifetime;
    }
}
using Infrastructure.Contexts;
using Infrastructure.Persistence.Enums;
using Infrastructure.Persistence.Options;
using Microsoft.EntityFrameworkCore;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Common;

namespace Infrastructure.Persistence.Contracts.Base;

public abstract class UnitOfWorkBase : IUnitOfWorkBase
{
    protected readonly Option _options;

    protected UnitOfWorkBase(Option options, AppDbContext context)
    {
        _options = options ?? 
                   throw new ArgumentNullException(nameof(options));
        
        _context = context ?? 
                   throw new ArgumentNullException(nameof(context));
    }

    public bool IsDisposed { get; protected set; }

    private AppDbContext? _context;

    internal AppDbContext DatabaseContext
    {
        get
        {
            _context ??= new AppDbContext(CreateOptions(_options),_options);

            return _context;
        }
    }

    public static DbContextOptions<AppDbContext> CreateOptions(Option option)
    {
        ArgumentNullException.ThrowIfNull(option);

        var optionsBuilder = 
            new DbContextOptionsBuilder<AppDbContext>();

        switch (option.Provider) // <-- lowercase 'option'
        {
            case Provider.SqlServer:
                optionsBuilder.UseSqlServer(option.ConnectionString);
                break;

            case Provider.MySql:
                optionsBuilder.UseMySQL(option.ConnectionString);
                break;

            case Provider.Oracle:
                optionsBuilder.UseOracle(option.ConnectionString);
                break;

            case Provider.PostgresDb:
                optionsBuilder.UseNpgsql(option.ConnectionString);
                break;

            case Provider.InMemory:
                optionsBuilder.UseInMemoryDatabase("ShoppingSystem_InMemoryDb");
                break;

            default:
                throw new NotSupportedException(
                    $"Provider '{option.Provider}' is not supported.");
        }

        return optionsBuilder.Options;
    }

    public void Save(CancellationToken ct)
    {
        DatabaseContext.SaveChanges();
    }

    public async Task SaveAsync(CancellationToken ct)
    {
        await DatabaseContext.SaveChangesAsync(ct);
    }

    public BaseRepository<T> GetRepository<T>()
        where T : BaseEntity
    {
        return new BaseRepository<T>(DatabaseContext);
    }

    public void Dispose()
    {
        Dispose(true);

        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (IsDisposed)
            return;

        if (disposing)
        {
            _context?.Dispose();
            _context = null;
        }

        IsDisposed = true;
    }

    ~UnitOfWorkBase()
    {
        Dispose(false);
    }
}
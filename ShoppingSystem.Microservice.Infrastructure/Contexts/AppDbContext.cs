using System.Linq.Expressions;
using System.Reflection;
using Infrastructure.Persistence.Enums;
using Infrastructure.Persistence.Options;
using Microsoft.EntityFrameworkCore;
using ShoppingSystem.Microservice.Domain.Aggregates.Cart;
using ShoppingSystem.Microservice.Domain.Aggregates.Inventory;
using ShoppingSystem.Microservice.Domain.Aggregates.Order;
using ShoppingSystem.Microservice.Domain.Aggregates.Payment;
using ShoppingSystem.Microservice.Domain.Aggregates.Product;
using ShoppingSystem.Microservice.Domain.Common;
using ShoppingSystem.Microservice.Domain.Entities;

namespace Infrastructure.Contexts;

public class AppDbContext : DbContext
{
    private readonly Option _option;

    public AppDbContext
    (DbContextOptions options 
    , Option option) : base(options)
    {
        _option = option;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder
        .HasDefaultSchema("Shopping");
        
        builder
            .ApplyConfigurationsFromAssembly
            (Assembly.GetExecutingAssembly());
        
        ApplyProviderSpecificDefaults(builder);

        #region SoftDelete

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var property = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
                var condition = Expression.Equal(property, Expression.Constant(false));
                var lambda = Expression.Lambda(condition, parameter);

                builder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }

        #endregion
    }
    
    private void ApplyProviderSpecificDefaults(ModelBuilder modelBuilder)
    {
        var createdAtSql = _option.Provider switch
        {
            Provider.SqlServer => "SYSDATETIMEOFFSET()",
            Provider.PostgresDb => "CURRENT_TIMESTAMP",
            Provider.MySql => "CURRENT_TIMESTAMP",
            Provider.Oracle => "SYSTIMESTAMP",
            _ => null
        };

        var isMainFilter = _option.Provider switch
        {
            Provider.SqlServer => "[IsMain] = 1",
            Provider.PostgresDb => "\"IsMain\" = TRUE",
            Provider.MySql => "`IsMain` = TRUE",
            Provider.Oracle => "\"IsMain\" = 1",
            _ => null
        };

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            // CreatedAt
            if (createdAtSql is not null)
            {
                entityType.FindProperty(nameof(BaseEntity.CreatedAt))
                    ?.SetDefaultValueSql(createdAtSql);
            }

            // ProductImage filtered index
            if (isMainFilter is not null &&
                entityType.ClrType == typeof(ProductImage))
            {
                var index = entityType.GetIndexes()
                    .FirstOrDefault(i =>
                        i.Properties.Select(p => p.Name)
                            .SequenceEqual(new[] { "ProductId", "IsMain" }));

                index?.SetFilter(isMainFilter);
            }
        }
    }

    #region DbSets

    #region Aggregates
    public DbSet<Cart> Cart { get; set; }
    public DbSet<Inventory> Inventory { get; set; }
    public DbSet<Order> Order { get; set; }
    public DbSet<Payment> Payment { get; set; }
    public DbSet<Category> Category { get; set; }
    public DbSet<Product> Product { get; set; }
    #endregion
    
    #region Entities
    public DbSet<Address> Address { get; set; }
    public DbSet<CartItem> CartItem { get; set; }
    public DbSet<OrderItem> OrderItem { get; set; }
    public DbSet<ProductImage> ProductImage { get; set; }
    #endregion
    
    #endregion DbSets
}
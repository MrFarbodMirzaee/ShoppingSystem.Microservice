using Infrastructure.Persistence.Comment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingSystem.Microservice.Domain.Aggregates.Inventory;
using ShoppingSystem.Microservice.Domain.ValueObjects;
using ShoppingSystem.Microservice.Resource;

namespace Infrastructure.Persistence.Configuration.Aggregates;

public class InventoryConfig
    : IEntityTypeConfiguration<Inventory>
{
    public void Configure(EntityTypeBuilder<Inventory> builder)
    {
        builder.ToTable(nameof(Inventory), table =>
        {
            table.HasComment(
                ResourcesComment.GetComment(nameof(DataDictionaries),nameof(DataDictionaries.Inventory))
            );
        });

        builder.HasKey(x => x.Id)
            .IsClustered(false);

        builder.Property(x => x.Id)
            .HasComment(ResourcesComment.GetComment(nameof(PropertyDataDictionary),nameof(PropertyDataDictionary.Id)));

        builder.Property(x => x.ProductId)
            .IsRequired()
            .HasComment(ResourcesComment.GetComment(nameof(PropertyDataDictionary),nameof(PropertyDataDictionary.ProductId)));

        builder.Property(x => x.Status)
            .IsRequired()
            .HasComment(ResourcesComment.GetComment(nameof(PropertyDataDictionary),nameof(PropertyDataDictionary.Status)));

        builder.Property(x => x.Quantity)
            .HasConversion(
                quantity => quantity.Value,
                value => StockQuantity.Create(value))
            .IsRequired()
            .HasComment(ResourcesComment.GetComment(nameof(PropertyDataDictionary),nameof(PropertyDataDictionary.Quantity)));

        builder.HasIndex(x => x.ProductId)
            .IsUnique();

        builder.HasOne(x => x.Product)
            .WithOne(p => p.Inventory)
            .HasForeignKey<Inventory>(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.CreatedAt)
            .HasComment(ResourcesComment.GetComment(nameof(PropertyDataDictionary),nameof(PropertyDataDictionary.CreatedAt)));
        
        builder.Property(x => x.IsDeleted)
            .HasComment(ResourcesComment.GetComment(nameof(PropertyDataDictionary),nameof(PropertyDataDictionary.IsDeleted)));
    }
}
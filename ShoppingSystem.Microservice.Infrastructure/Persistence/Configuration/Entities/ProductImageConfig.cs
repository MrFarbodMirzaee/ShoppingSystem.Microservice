using Infrastructure.Persistence.Comment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingSystem.Microservice.Domain.Entities;
using ShoppingSystem.Microservice.Resource;

namespace Infrastructure.Persistence.Configuration.Entities;

public class ProductImageConfig 
    : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.ToTable(nameof(ProductImage), table =>
        {
            table.HasComment(
                ResourcesComment.GetComment(nameof(DataDictionaries),nameof(DataDictionaries.ProductImage))
            );
        });

        builder.HasKey(x => x.Id)
            .IsClustered(false);

        builder.Property(x => x.Id)
            .HasComment(ResourcesComment.GetComment(nameof(PropertyDataDictionary),nameof(PropertyDataDictionary.Id)));

        builder.Property(x => x.ProductId)
            .IsRequired()
            .HasComment(ResourcesComment.GetComment(nameof(PropertyDataDictionary),nameof(PropertyDataDictionary.ProductId)));

        builder.Property(x => x.FileName)
            .IsRequired()
            .HasMaxLength(255)
            .HasComment(ResourcesComment.GetComment(nameof(PropertyDataDictionary),nameof(PropertyDataDictionary.FileName)));

        builder.Property(x => x.FilePath)
            .IsRequired()
            .HasMaxLength(500)
            .HasComment(ResourcesComment.GetComment(nameof(PropertyDataDictionary),nameof(PropertyDataDictionary.FilePath)));

        builder.Property(x => x.ContentType)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment(ResourcesComment.GetComment(nameof(PropertyDataDictionary),nameof(PropertyDataDictionary.ContentType)));

        builder.Property(x => x.FileSize)
            .IsRequired()
            .HasComment(ResourcesComment.GetComment(nameof(PropertyDataDictionary),nameof(PropertyDataDictionary.FileSize)));

        builder.Property(x => x.IsMain)
            .IsRequired()
            .HasComment(ResourcesComment.GetComment(nameof(PropertyDataDictionary),nameof(PropertyDataDictionary.IsMain)));

        builder.HasIndex(x => x.ProductId);

        builder.HasIndex(x => new
            {
                x.ProductId,
                x.IsMain
            })
            .IsUnique();

        builder.Property(x => x.CreatedAt)
            .HasComment(ResourcesComment.GetComment(nameof(PropertyDataDictionary),nameof(PropertyDataDictionary.CreatedAt)));
        
        builder.Property(x => x.IsDeleted)
            .HasComment(ResourcesComment.GetComment(nameof(PropertyDataDictionary),nameof(PropertyDataDictionary.IsDeleted)));
    }
}
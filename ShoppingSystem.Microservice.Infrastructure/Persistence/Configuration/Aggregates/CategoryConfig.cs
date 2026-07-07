using Infrastructure.Persistence.Comment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingSystem.Microservice.Domain.Aggregates.Product;
using ShoppingSystem.Microservice.Resource;

namespace Infrastructure.Persistence.Configuration.Aggregates;

public class CategoryConfig
    : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable(nameof(Category), table =>
        {
            table.HasComment(
                ResourcesComment.GetComment(nameof(DataDictionaries),nameof(DataDictionaries.Category))
            );
        });

        builder.HasKey(x => x.Id)
            .IsClustered(false);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment(ResourcesComment.GetComment(nameof(PropertyDataDictionary),nameof(PropertyDataDictionary.Name)));

        builder.Property(x => x.Description)
            .HasMaxLength(500)
            .HasComment(ResourcesComment.GetComment(nameof(PropertyDataDictionary),nameof(PropertyDataDictionary.Description)));

        builder.HasIndex(x => x.Name);

        builder.HasIndex(x => x.ParentCategoryId);

        builder.Property(x => x.ParentCategoryId)
            .IsRequired(false)
            .HasComment(ResourcesComment.GetComment(nameof(PropertyDataDictionary),nameof(PropertyDataDictionary.ParentCategoryId)));

        builder.Property(x => x.IsActive)
            .IsRequired()
            .HasComment(ResourcesComment.GetComment(nameof(PropertyDataDictionary),nameof(PropertyDataDictionary.IsActive)));

        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(x => x.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.CreatedAt)
            .HasComment(ResourcesComment.GetComment(nameof(PropertyDataDictionary),nameof(PropertyDataDictionary.CreatedAt)));
        
        builder.Property(x => x.IsDeleted)
            .HasComment(ResourcesComment.GetComment(nameof(PropertyDataDictionary),nameof(PropertyDataDictionary.IsDeleted)));
    }
}
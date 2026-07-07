using Infrastructure.Persistence.Comment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingSystem.Microservice.Domain.Entities;
using ShoppingSystem.Microservice.Resource;

namespace Infrastructure.Persistence.Configuration.Entities;

public class AddressConfig
    : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable(nameof(Address), table =>
        {
            table.HasComment(
                ResourcesComment.GetComment(nameof(DataDictionaries),nameof(DataDictionaries.Address))
            );
        });

        builder.HasKey(x => x.Id)
            .IsClustered(false);

        builder.Property(x => x.Id)
            .HasComment(ResourcesComment.GetComment(nameof(PropertyDataDictionary),nameof(PropertyDataDictionary.Id)));

        builder.Property(x => x.Street)
            .IsRequired()
            .HasMaxLength(200)
            .HasComment(ResourcesComment.GetComment(nameof(PropertyDataDictionary),nameof(PropertyDataDictionary.Street)));

        builder.Property(x => x.City)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment(ResourcesComment.GetComment(nameof(PropertyDataDictionary),nameof(PropertyDataDictionary.City)));

        builder.Property(x => x.State)
            .HasMaxLength(100)
            .HasComment(ResourcesComment.GetComment(nameof(PropertyDataDictionary),nameof(PropertyDataDictionary.State)));

        builder.Property(x => x.Country)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment(ResourcesComment.GetComment(nameof(PropertyDataDictionary),nameof(PropertyDataDictionary.Country)));

        builder.Property(x => x.PostalCode)
            .IsRequired()
            .HasMaxLength(20)
            .HasComment(ResourcesComment.GetComment(nameof(PropertyDataDictionary),nameof(PropertyDataDictionary.PostalCode)));

        builder.Property(x => x.CreatedAt)
            .HasComment(ResourcesComment.GetComment(nameof(PropertyDataDictionary),nameof(PropertyDataDictionary.CreatedAt)));
        
        builder.Property(x => x.IsDeleted)
            .HasComment(ResourcesComment.GetComment(nameof(PropertyDataDictionary),nameof(PropertyDataDictionary.IsDeleted)));
    }
}
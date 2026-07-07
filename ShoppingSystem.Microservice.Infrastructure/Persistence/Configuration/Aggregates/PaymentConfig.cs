using Infrastructure.Persistence.Comment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingSystem.Microservice.Domain.Aggregates.Payment;
using ShoppingSystem.Microservice.Domain.ValueObjects;
using ShoppingSystem.Microservice.Resource;

namespace Infrastructure.Persistence.Configuration.Aggregates;

public class PaymentConfig
    : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable(nameof(Payment), table =>
        {
            table.HasComment(
                ResourcesComment.GetComment(nameof(DataDictionaries),nameof(DataDictionaries.Payment))
            );
        });

        builder.HasKey(x => x.Id)
            .IsClustered(false);

        builder.Property(x => x.Id)
            .HasComment(ResourcesComment.GetComment(nameof(PropertyDataDictionary),nameof(PropertyDataDictionary.Id)));

        builder.Property(x => x.OrderId)
            .IsRequired()
            .HasComment(ResourcesComment.GetComment(nameof(PropertyDataDictionary),nameof(PropertyDataDictionary.OrderId)));

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired()
            .HasComment(ResourcesComment.GetComment(nameof(PropertyDataDictionary),nameof(PropertyDataDictionary.Status)));

        builder.Property(x => x.PaidAt)
            .IsRequired(false)
            .HasComment(ResourcesComment.GetComment(nameof(PropertyDataDictionary),nameof(PropertyDataDictionary.PaidAt)));

        builder.Property(x => x.CreatedAt)
            .HasComment(ResourcesComment.GetComment(nameof(PropertyDataDictionary),nameof(PropertyDataDictionary.CreatedAt)));

        builder.OwnsOne(
            x => x.Amount,
            money =>
            {
                money.Property(x => x.Amount)
                    .HasColumnName("Amount")
                    .HasPrecision(18, 2)
                    .IsRequired()
                    .HasComment(ResourcesComment.GetComment(nameof(PropertyDataDictionary),nameof(PropertyDataDictionary.Amount)));

                money.Property(x => x.Currency)
                    .HasColumnName("Currency")
                    .HasMaxLength(3)
                    .IsRequired()
                    .HasComment(ResourcesComment.GetComment(nameof(PropertyDataDictionary),nameof(PropertyDataDictionary.Currency)));
            });

        builder.Property(x => x.TransactionId)
            .HasConversion(
                x => x == null ? null : x.Value,
                x => x == null ? null : TransactionId.Create(x))
            .HasMaxLength(100)
            .IsRequired(false)
            .HasComment(ResourcesComment.GetComment(nameof(PropertyDataDictionary),nameof(PropertyDataDictionary.TransactionId)));
        
        builder.Property(x => x.IsDeleted)
            .HasComment(ResourcesComment.GetComment(nameof(PropertyDataDictionary),nameof(PropertyDataDictionary.IsDeleted)));

        builder.HasIndex(x => x.OrderId)
            .IsUnique();

        builder.HasOne(p => p.Order)
            .WithMany(o => o.Payments)
            .HasForeignKey(p => p.OrderId);
    }
}
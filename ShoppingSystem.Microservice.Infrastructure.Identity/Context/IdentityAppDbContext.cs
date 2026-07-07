using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShoppingSystem.Microservice.Infrastructure.Identity.Persistence.Entities;

namespace ShoppingSystem.Microservice.Infrastructure.Identity.Context;

public class IdentityAppDbContext 
    : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public IdentityAppDbContext(
        DbContextOptions<IdentityAppDbContext> options)
        : base(options)
    {
    }


    protected override void OnModelCreating(
        ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("Identity");
        // Custom Identity table configurations
        builder.Entity<ApplicationUser>()
            .ToTable("Users");

        builder.Entity<IdentityRole<Guid>>()
            .ToTable("Roles");

        builder.Entity<IdentityUserRole<Guid>>()
            .ToTable("UserRoles");

        builder.Entity<IdentityUserClaim<Guid>>()
            .ToTable("UserClaims");

        builder.Entity<IdentityUserLogin<Guid>>()
            .ToTable("UserLogins");

        builder.Entity<IdentityRoleClaim<Guid>>()
            .ToTable("RoleClaims");

        builder.Entity<IdentityUserToken<Guid>>()
            .ToTable("UserTokens");
        
        builder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("RefreshTokens");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Token)
                .IsRequired()
                .HasMaxLength(512);

            entity.Property(x => x.ExpiresAt)
                .IsRequired();

            entity.Property(x => x.IsRevoked)
                .IsRequired();

            entity.Property(x => x.RevokedAt);

            entity.HasOne(x => x.User)
                .WithMany(x => x.RefreshTokens)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
}
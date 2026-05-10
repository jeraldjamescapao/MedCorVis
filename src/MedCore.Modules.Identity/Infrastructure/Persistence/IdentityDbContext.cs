namespace MedCore.Modules.Identity.Infrastructure.Persistence;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MedCore.Modules.Identity.Domain.Roles;
using MedCore.Modules.Identity.Domain.Tokens; 
using MedCore.Modules.Identity.Domain.Users;

internal sealed class IdentityDbContext 
    : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("Identity");

        builder.Entity<ApplicationUser>(entity =>
        {
            entity.ToTable("Users");

            entity.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(x => x.BirthDate)
                .HasColumnType("date")
                .IsRequired();
            
            entity.Property(x => x.PreferredCulture)
                .HasMaxLength(10)
                .IsRequired(false);

            entity.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            entity.Property(x => x.CreatedAtUtc)
                .HasColumnType("datetimeoffset")
                .IsRequired();

            entity.Property(x => x.ModifiedAtUtc)
                .HasColumnType("datetimeoffset");
            
            entity.Property(x => x.CreatedBy)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.ModifiedBy)
                .HasMaxLength(100);

            entity.HasIndex(x => x.IsActive)
                .HasDatabaseName("IX_Users_Is_Active");
        });

        builder.Entity<ApplicationRole>(entity =>
        {
            entity.ToTable("Roles");
            
            entity.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(250);
        });

        builder.Entity<IdentityUserRole<Guid>>(entity =>
        {
            entity.ToTable("UserRoles");
        });

        builder.Entity<IdentityUserClaim<Guid>>(entity =>
        {
            entity.ToTable("UserClaims");
        });

        builder.Entity<IdentityUserLogin<Guid>>(entity =>
        {
            entity.ToTable("UserLogins");
        });

        builder.Entity<IdentityRoleClaim<Guid>>(entity =>
        {
            entity.ToTable("RoleClaims");
        });

        builder.Entity<IdentityUserToken<Guid>>(entity =>
        {
            entity.ToTable("UserTokens");
        });
        
        builder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("RefreshTokens");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Token)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(x => x.ExpiresAtUtc)
                .HasColumnType("datetimeoffset")
                .IsRequired();

            entity.Property(x => x.CreatedAtUtc)
                .HasColumnType("datetimeoffset")
                .IsRequired();

            entity.Property(x => x.IsRevoked)
                .IsRequired()
                .HasDefaultValue(false);

            entity.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasIndex(x => x.Token)
                .IsUnique();

            entity.HasIndex(x => x.FamilyId);

            entity.HasIndex(x => x.UserId);
        });
    }
}
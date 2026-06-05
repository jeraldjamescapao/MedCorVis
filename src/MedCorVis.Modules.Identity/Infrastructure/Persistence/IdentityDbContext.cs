namespace MedCorVis.Modules.Identity.Infrastructure.Persistence;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MedCorVis.Common.Auditing;
using MedCorVis.Modules.Identity.Domain.Roles;
using MedCorVis.Modules.Identity.Domain.Tokens;
using MedCorVis.Modules.Identity.Domain.Users;

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
            
            entity.Property(x => x.PreferredCulture)
                .HasMaxLength(ApplicationUser.PreferredCultureMaxLength)
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
                .HasMaxLength(IAuditableEntity.CreatedByMaxLength);

            entity.Property(x => x.ModifiedBy)
                .HasMaxLength(IAuditableEntity.ModifiedByMaxLength);

            entity.HasIndex(x => x.IsActive)
                .HasDatabaseName("IX_Users_Is_Active");
            
            entity.Property(x => x.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            entity.Property(x => x.DeletedAtUtc)
                .HasColumnType("datetimeoffset");

            entity.Property(x => x.DeletedBy)
                .HasMaxLength(IAuditableEntity.ModifiedByMaxLength);

            entity.Property(x => x.DeletionRequestedAtUtc)
                .HasColumnType("datetimeoffset");

            entity.HasIndex(x => x.DeletionRequestedAtUtc)
                .HasDatabaseName("IX_Users_DeletionRequestedAtUtc");

            entity.HasQueryFilter(u => !u.IsDeleted);
        });

        builder.Entity<ApplicationRole>(entity =>
        {
            entity.ToTable("Roles");
            
            entity.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(ApplicationRole.MaxDescriptionLength);
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
                .HasMaxLength(RefreshToken.TokenMaxLength);

            entity.Property(x => x.ExpiresAtUtc)
                .HasColumnType("datetimeoffset")
                .IsRequired();

            entity.Property(x => x.CreatedAtUtc)
                .HasColumnType("datetimeoffset")
                .IsRequired();

            entity.Property(x => x.IsRevoked)
                .IsRequired()
                .HasDefaultValue(false);
            
            entity.Property(x => x.RevokedAtUtc)
                .HasColumnType("datetimeoffset");

            entity.Property(x => x.IpAddress)
                .HasMaxLength(RefreshToken.IpAddressMaxLength);

            entity.Property(x => x.UserAgent)
                .HasMaxLength(RefreshToken.UserAgentMaxLength);

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
namespace MedCore.Infrastructure.Localization;

using Microsoft.EntityFrameworkCore;

internal sealed class LocalizationDbContext : DbContext
{
    public DbSet<Translation> Translations => Set<Translation>();

    public LocalizationDbContext(DbContextOptions<LocalizationDbContext> options)
        : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("Localization");

        builder.Entity<Translation>(entity =>
        {
            entity.ToTable("Translations");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .UseIdentityColumn();

            entity.Property(x => x.Culture)
                .IsRequired()
                .HasMaxLength(10);

            entity.Property(x => x.Key)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(x => x.Value)
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            entity.HasIndex(x => x.Culture)
                .HasDatabaseName("IX_Translations_Culture");

            entity.HasIndex(x => new { x.Culture, x.Key })
                .IsUnique()
                .HasDatabaseName("IX_Translations_Culture_Key");
        });
    }
}
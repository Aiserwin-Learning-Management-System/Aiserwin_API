namespace Winfocus.LMS.Infrastructure.Persistence.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// EF Core configuration for <see cref="FieldOption"/>.
    /// Standalone configuration (does NOT inherit BaseEntityConfiguration)
    /// because FieldOption is a lightweight child entity without audit fields.
    /// </summary>
    public class FieldOptionConfiguration : IEntityTypeConfiguration<FieldOption>
    {
        /// <summary>
        /// Configures the entity of type <typeparamref name="TEntity" />.
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity type.</param>
        public void Configure(EntityTypeBuilder<FieldOption> builder)
        {
            // ── Table ────────────────────────────────────────────
            builder.ToTable("FieldOptions");

            // ── Primary Key ──────────────────────────────────────
            builder.HasKey(fo => fo.Id);

            builder.Property(fo => fo.Id)
                .HasColumnType("uniqueidentifier")
                .HasDefaultValueSql("NEWSEQUENTIALID()")
                .ValueGeneratedOnAdd();

            // ── FieldId (required FK) ────────────────────────────
            builder.Property(fo => fo.FieldId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            // ── OptionValue ──────────────────────────────────────
            builder.Property(fo => fo.OptionValue)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("nvarchar(200)");

            // ── DisplayOrder ─────────────────────────────────────
            builder.Property(fo => fo.DisplayOrder)
                .IsRequired()
                .HasDefaultValue(0)
                .HasColumnType("int");

            // ── IsActive ─────────────────────────────────────────
            builder.Property(fo => fo.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .HasColumnType("bit");

            // ── Indexes ──────────────────────────────────────────
            // Ordered options within a field
            builder.HasIndex(fo => new { fo.FieldId, fo.DisplayOrder })
                .HasDatabaseName("IX_FieldOptions_FieldId_DisplayOrder");

            // Prevent duplicate option values within same field
            builder.HasIndex(fo => new { fo.FieldId, fo.OptionValue })
                .IsUnique()
                .HasDatabaseName("IX_FieldOptions_FieldId_OptionValue_Unique");
        }
    }
}

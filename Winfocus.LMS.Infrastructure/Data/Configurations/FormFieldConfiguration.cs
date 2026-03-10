namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// EF Core configuration for <see cref="FormField"/>.
    /// Inherits common column setup from <see cref="BaseEntityConfiguration{TEntity}"/>.
    /// </summary>
    public class FormFieldConfiguration : BaseEntityConfiguration<FormField>
    {
        /// <summary>
        /// Override in derived configurations to add entity-specific
        /// columns, indexes, relationships, and constraints.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void ConfigureEntity(EntityTypeBuilder<FormField> builder)
        {
            // ── Table ────────────────────────────────────────────
            builder.ToTable("FormFields");

            // ── FieldGroupId (nullable FK) ───────────────────────
            builder.Property(ff => ff.FieldGroupId)
                .IsRequired(false)
                .HasColumnType("uniqueidentifier");

            // ── FieldName ────────────────────────────────────────
            builder.Property(ff => ff.FieldName)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("nvarchar(100)");

            // ── DisplayLabel ─────────────────────────────────────
            builder.Property(ff => ff.DisplayLabel)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("nvarchar(200)");

            // ── Placeholder ──────────────────────────────────────
            builder.Property(ff => ff.Placeholder)
                .IsRequired(false)
                .HasMaxLength(200)
                .HasColumnType("nvarchar(200)");

            // ── FieldType (enum → int) ───────────────────────────
            builder.Property(ff => ff.FieldType)
                .IsRequired()
                .HasConversion<int>()
                .HasColumnType("int");

            // ── IsRequired ───────────────────────────────────────
            builder.Property(ff => ff.IsRequired)
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnType("bit");

            // ── ValidationRegex ──────────────────────────────────
            builder.Property(ff => ff.ValidationRegex)
                .IsRequired(false)
                .HasMaxLength(500)
                .HasColumnType("nvarchar(500)");

            // ── MinLength ────────────────────────────────────────
            builder.Property(ff => ff.MinLength)
                .IsRequired(false)
                .HasColumnType("int");

            // ── MaxLength ────────────────────────────────────────
            builder.Property(ff => ff.MaxLength)
                .IsRequired(false)
                .HasColumnType("int");

            // ── DisplayOrder ─────────────────────────────────────
            builder.Property(ff => ff.DisplayOrder)
                .IsRequired()
                .HasDefaultValue(0)
                .HasColumnType("int");

            // ── Indexes ──────────────────────────────────────────
            // Unique FieldName among non-deleted records
            builder.HasIndex(ff => ff.FieldName)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0")
                .HasDatabaseName("IX_FormFields_FieldName_WhereNotDeleted");

            // Composite index for ordered queries within a group
            builder.HasIndex(ff => new { ff.FieldGroupId, ff.DisplayOrder })
                .HasDatabaseName("IX_FormFields_GroupId_DisplayOrder");

            // ── Relationships ────────────────────────────────────
            // FormField → FieldOptions (one-to-many, cascade delete)
            builder.HasMany(ff => ff.FieldOptions)
                .WithOne(fo => fo.FormField)
                .HasForeignKey(fo => fo.FieldId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

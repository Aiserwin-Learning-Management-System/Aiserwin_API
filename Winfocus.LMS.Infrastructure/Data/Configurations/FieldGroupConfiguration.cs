namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Data.Configurations;

    /// <summary>
    /// EF Core configuration for <see cref="FieldGroup"/>.
    /// Inherits common column setup from <see cref="BaseEntityConfiguration{TEntity}"/>.
    /// </summary>
    public class FieldGroupConfiguration : BaseEntityConfiguration<FieldGroup>
    {
        /// <summary>
        /// Override in derived configurations to add entity-specific
        /// columns, indexes, relationships, and constraints.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void ConfigureEntity(EntityTypeBuilder<FieldGroup> builder)
        {
            // ── Table ────────────────────────────────────────────
            builder.ToTable("FieldGroups");

            // ── GroupName ────────────────────────────────────────
            builder.Property(fg => fg.GroupName)
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnType("nvarchar(150)");

            // ── Description ──────────────────────────────────────
            builder.Property(fg => fg.Description)
                .IsRequired(false)
                .HasMaxLength(500)
                .HasColumnType("nvarchar(500)");

            // ── DisplayOrder ─────────────────────────────────────
            builder.Property(fg => fg.DisplayOrder)
                .IsRequired()
                .HasDefaultValue(0)
                .HasColumnType("int");

            // ── Indexes ──────────────────────────────────────────
            // Unique GroupName among non-deleted records
            builder.HasIndex(fg => fg.GroupName)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0")
                .HasDatabaseName("IX_FieldGroups_GroupName_WhereNotDeleted");

            // Fast ordering queries
            builder.HasIndex(fg => fg.DisplayOrder)
                .HasDatabaseName("IX_FieldGroups_DisplayOrder");

            // ── Relationships ────────────────────────────────────
            // FieldGroup → FormFields (one-to-many)
            builder.HasMany(fg => fg.FormFields)
                .WithOne(ff => ff.FieldGroup)
                .HasForeignKey(ff => ff.FieldGroupId)
                .IsRequired(false) // nullable FK
                .OnDelete(DeleteBehavior.SetNull); // ungrouped if group deleted
        }
    }
}

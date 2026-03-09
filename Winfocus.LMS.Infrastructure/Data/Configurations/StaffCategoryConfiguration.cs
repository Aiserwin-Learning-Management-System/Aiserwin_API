namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// EF Core configuration specific to <see cref="StaffCategory"/>.
    /// Common BaseEntity properties are handled by <see cref="BaseEntityConfiguration{TEntity}"/>.
    /// </summary>
    public class StaffCategoryConfiguration : BaseEntityConfiguration<StaffCategory>
    {
        /// <summary>
        /// Override in derived configurations to add entity-specific
        /// columns, indexes, relationships, and constraints.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void ConfigureEntity(EntityTypeBuilder<StaffCategory> builder)
        {
            // ── Table ────────────────────────────────────────────
            builder.ToTable("StaffCategories");

            // ── Name ─────────────────────────────────────────────
            builder.Property(sc => sc.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("nvarchar(100)");

            // ── Description ──────────────────────────────────────
            builder.Property(sc => sc.Description)
                .IsRequired(false)
                .HasMaxLength(500)
                .HasColumnType("nvarchar(500)");

            // ── Indexes ──────────────────────────────────────────
            builder.HasIndex(sc => sc.Name)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0")
                .HasDatabaseName("IX_StaffCategories_Name_WhereNotDeleted");

            builder.HasIndex(sc => sc.IsActive)
                .HasDatabaseName("IX_StaffCategories_IsActive");
        }
    }
}

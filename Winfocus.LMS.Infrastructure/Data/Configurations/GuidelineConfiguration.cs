namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Data.Configurations;

    /// <summary>
    /// Uses BaseEntityConfiguration for soft delete, audit fields, query filter.
    /// </summary>
    public class GuidelineConfiguration : BaseEntityConfiguration<Guideline>
    {
        /// <summary>
        /// Override in derived configurations to add entity-specific
        /// columns, indexes, relationships, and constraints.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void ConfigureEntity(EntityTypeBuilder<Guideline> builder)
        {
            builder.ToTable("Guidelines");

            // ── Title ────────────────────────────────────────────
            builder.Property(g => g.Title)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("nvarchar(200)");

            // ── Content (HTML) ───────────────────────────────────
            builder.Property(g => g.Content)
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            // ── Category ─────────────────────────────────────────
            builder.Property(g => g.Category)
                .IsRequired(false)
                .HasMaxLength(100)
                .HasColumnType("nvarchar(100)");

            // ── DisplayOrder ─────────────────────────────────────
            builder.Property(g => g.DisplayOrder)
                .IsRequired()
                .HasDefaultValue(0)
                .HasColumnType("int");

            // ── Indexes ──────────────────────────────────────────
            builder.HasIndex(g => new { g.Category, g.DisplayOrder })
                .HasDatabaseName("IX_Guidelines_Category_DisplayOrder");

            builder.HasIndex(g => g.Title)
                .HasDatabaseName("IX_Guidelines_Title");
        }
    }
}

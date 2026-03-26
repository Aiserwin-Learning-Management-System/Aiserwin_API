namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Data.Configurations;

    /// <summary>
    /// EF Core configuration for <see cref="PageHeading"/>.
    /// Seed data handled by PageHeadingSeeder (not HasData)
    /// because CreatedBy depends on SuperAdmin user created at runtime.
    /// </summary>
    public class PageHeadingConfiguration : BaseEntityConfiguration<PageHeading>
    {
        /// <summary>
        /// Override in derived configurations to add entity-specific
        /// columns, indexes, relationships, and constraints.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void ConfigureEntity(EntityTypeBuilder<PageHeading> builder)
        {
            // ── Table ────────────────────────────────────────────
            builder.ToTable("PageHeadings");

            // ── PageKey ──────────────────────────────────────────
            builder.Property(ph => ph.PageKey)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("nvarchar(50)");

            // ── MainHeading ──────────────────────────────────────
            builder.Property(ph => ph.MainHeading)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("nvarchar(200)");

            // ── SubHeading ───────────────────────────────────────
            builder.Property(ph => ph.SubHeading)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("nvarchar(200)");

            // ── ModuleName ───────────────────────────────────────
            builder.Property(ph => ph.ModuleName)
                .IsRequired()
                .HasMaxLength(100)
                .HasDefaultValue("User Management")
                .HasColumnType("nvarchar(100)");

            // ── Indexes ──────────────────────────────────────────
            builder.HasIndex(ph => ph.PageKey)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0")
                .HasDatabaseName("IX_PageHeadings_PageKey_Unique");

            builder.HasIndex(ph => ph.ModuleName)
                .HasDatabaseName("IX_PageHeadings_ModuleName");
        }
    }
}

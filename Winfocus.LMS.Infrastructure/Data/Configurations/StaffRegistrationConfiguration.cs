namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Domain.Enums;

    /// <summary>
    /// EF Core configuration for <see cref="StaffRegistration"/>.
    /// Now uses BaseEntityConfiguration for Id, audit fields, IsActive, IsDeleted, query filter.
    /// </summary>
    public class StaffRegistrationConfiguration : BaseEntityConfiguration<StaffRegistration>
    {
        /// <summary>
        /// Override in derived configurations to add entity-specific
        /// columns, indexes, relationships, and constraints.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void ConfigureEntity(EntityTypeBuilder<StaffRegistration> builder)
        {
            // ── Table ────────────────────────────────────────────
            builder.ToTable("StaffRegistrations");

            // ── FormId (required FK) ─────────────────────────────
            builder.Property(sr => sr.FormId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            // ── StaffCategoryId (required FK) ────────────────────
            builder.Property(sr => sr.StaffCategoryId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            // ── Status (enum → int) ──────────────────────────────
            builder.Property(sr => sr.Status)
                .IsRequired()
                .HasConversion<int>()
                .HasDefaultValue(RegistrationStatus.Draft)
                .HasColumnType("int");

            // ── Remarks ──────────────────────────────────────────
            builder.Property(sr => sr.Remarks)
                .IsRequired(false)
                .HasMaxLength(1000)
                .HasColumnType("nvarchar(1000)");

            // ── SubmittedAt ──────────────────────────────────────
            builder.Property(sr => sr.SubmittedAt)
                .IsRequired(false)
                .HasColumnType("datetime2");

            // ── Indexes ──────────────────────────────────────────
            builder.HasIndex(sr => sr.StaffCategoryId)
                .HasDatabaseName("IX_StaffRegistrations_StaffCategoryId");

            builder.HasIndex(sr => sr.FormId)
                .HasDatabaseName("IX_StaffRegistrations_FormId");

            builder.HasIndex(sr => sr.Status)
                .HasDatabaseName("IX_StaffRegistrations_Status");

            // ── Relationships ────────────────────────────────────

            builder.HasOne(sr => sr.StaffCategory)
                .WithMany()
                .HasForeignKey(sr => sr.StaffCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(sr => sr.Values)
                .WithOne(srv => srv.StaffRegistration)
                .HasForeignKey(srv => srv.RegistrationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

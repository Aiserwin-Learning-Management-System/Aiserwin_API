namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Domain.Enums;

    /// <summary>
    /// EF Core configuration for <see cref="StaffRegistration"/>.
    /// Standalone configuration — has its own audit pattern (no UpdatedAt/UpdatedBy).
    /// </summary>
    public class StaffRegistrationConfiguration : IEntityTypeConfiguration<StaffRegistration>
    {
        /// <summary>
        /// Configures the entity of type <typeparamref name="TEntity" />.
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity type.</param>
        public void Configure(EntityTypeBuilder<StaffRegistration> builder)
        {
            // ── Table ────────────────────────────────────────────
            builder.ToTable("StaffRegistrations");

            // ── Primary Key ──────────────────────────────────────
            builder.HasKey(sr => sr.Id);

            builder.Property(sr => sr.Id)
                .HasColumnType("uniqueidentifier")
                .HasDefaultValueSql("NEWSEQUENTIALID()")
                .ValueGeneratedOnAdd();

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

            // ── CreatedAt ────────────────────────────────────────
            builder.Property(sr => sr.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()")
                .HasColumnType("datetime2");

            // ── CreatedBy ────────────────────────────────────────
            builder.Property(sr => sr.CreatedBy)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            // ── Indexes ──────────────────────────────────────────

            // Find registrations by category
            builder.HasIndex(sr => sr.StaffCategoryId)
                .HasDatabaseName("IX_StaffRegistrations_StaffCategoryId");

            // Find registrations by form
            builder.HasIndex(sr => sr.FormId)
                .HasDatabaseName("IX_StaffRegistrations_FormId");

            // Filter by status
            builder.HasIndex(sr => sr.Status)
                .HasDatabaseName("IX_StaffRegistrations_Status");

            // ── Relationships ────────────────────────────────────

            // StaffRegistration → StaffCategory (many-to-one)
            builder.HasOne(sr => sr.StaffCategory)
                .WithMany()
                .HasForeignKey(sr => sr.StaffCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // StaffRegistration → StaffRegistrationValues (one-to-many, cascade)
            builder.HasMany(sr => sr.Values)
                .WithOne(srv => srv.StaffRegistration)
                .HasForeignKey(srv => srv.RegistrationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

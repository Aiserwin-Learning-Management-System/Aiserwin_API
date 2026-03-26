namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// EF Core configuration for <see cref="RegistrationForm"/>.
    /// Inherits common column setup from <see cref="BaseEntityConfiguration{TEntity}"/>.
    /// </summary>
    public class RegistrationFormConfiguration : BaseEntityConfiguration<RegistrationForm>
    {
        /// <summary>
        /// Override in derived configurations to add entity-specific
        /// columns, indexes, relationships, and constraints.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void ConfigureEntity(EntityTypeBuilder<RegistrationForm> builder)
        {
            // ── Table ────────────────────────────────────────────
            builder.ToTable("RegistrationForms");

            // ── StaffCategoryId (required FK) ────────────────────
            builder.Property(rf => rf.StaffCategoryId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            // ── FormName ─────────────────────────────────────────
            builder.Property(rf => rf.FormName)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("nvarchar(200)");

            // ── Description ──────────────────────────────────────
            builder.Property(rf => rf.Description)
                .IsRequired(false)
                .HasMaxLength(500)
                .HasColumnType("nvarchar(500)");

            // ── Indexes ──────────────────────────────────────────

            // One active form per staff category
            builder.HasIndex(rf => rf.StaffCategoryId)
                .IsUnique()
                .HasFilter("[IsActive] = 1 AND [IsDeleted] = 0")
                .HasDatabaseName("IX_RegistrationForms_OneActivePerCategory");

            // Search by form name
            builder.HasIndex(rf => rf.FormName)
                .HasDatabaseName("IX_RegistrationForms_FormName");

            // ── Relationships ────────────────────────────────────

            // RegistrationForm → StaffCategory (many-to-one)
            builder.HasOne(rf => rf.StaffCategory)
                .WithMany()
                .HasForeignKey(rf => rf.StaffCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // RegistrationForm → RegistrationFormGroups (one-to-many, cascade)
            builder.HasMany(rf => rf.FormGroups)
                .WithOne(rfg => rfg.RegistrationForm)
                .HasForeignKey(rfg => rfg.FormId)
                .OnDelete(DeleteBehavior.Cascade);

            // RegistrationForm → RegistrationFormFields (one-to-many, cascade)
            builder.HasMany(rf => rf.FormFields)
                .WithOne(rff => rff.RegistrationForm)
                .HasForeignKey(rff => rff.FormId)
                .OnDelete(DeleteBehavior.Cascade);

            // RegistrationForm → StaffRegistrations (one-to-many)
            builder.HasMany(rf => rf.Registrations)
                .WithOne(sr => sr.RegistrationForm)
                .HasForeignKey(sr => sr.FormId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

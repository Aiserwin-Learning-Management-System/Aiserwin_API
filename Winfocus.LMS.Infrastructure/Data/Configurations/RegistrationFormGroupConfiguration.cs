namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// EF Core configuration for <see cref="RegistrationFormGroup"/>.
    /// Standalone configuration — lightweight junction entity without BaseEntity.
    /// </summary>
    public class RegistrationFormGroupConfiguration : IEntityTypeConfiguration<RegistrationFormGroup>
    {
        /// <summary>
        /// Configures the entity of type <typeparamref name="TEntity" />.
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity type.</param>
        public void Configure(EntityTypeBuilder<RegistrationFormGroup> builder)
        {
            // ── Table ────────────────────────────────────────────
            builder.ToTable("RegistrationFormGroups");

            // ── Primary Key ──────────────────────────────────────
            builder.HasKey(rfg => rfg.Id);

            builder.Property(rfg => rfg.Id)
                .HasColumnType("uniqueidentifier")
                .HasDefaultValueSql("NEWSEQUENTIALID()")
                .ValueGeneratedOnAdd();

            // ── FormId (required FK) ─────────────────────────────
            builder.Property(rfg => rfg.FormId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            // ── FieldGroupId (required FK) ───────────────────────
            builder.Property(rfg => rfg.FieldGroupId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            // ── DisplayOrder ─────────────────────────────────────
            builder.Property(rfg => rfg.DisplayOrder)
                .IsRequired()
                .HasDefaultValue(0)
                .HasColumnType("int");

            // ── Indexes ──────────────────────────────────────────

            // No duplicate groups in same form
            builder.HasIndex(rfg => new { rfg.FormId, rfg.FieldGroupId })
                .IsUnique()
                .HasDatabaseName("IX_RegFormGroups_FormId_FieldGroupId_Unique");

            // Ordered groups within a form
            builder.HasIndex(rfg => new { rfg.FormId, rfg.DisplayOrder })
                .HasDatabaseName("IX_RegFormGroups_FormId_DisplayOrder");

            // ── Relationships ────────────────────────────────────

            // RegistrationFormGroup → FieldGroup (many-to-one)
            builder.HasOne(rfg => rfg.FieldGroup)
                .WithMany()
                .HasForeignKey(rfg => rfg.FieldGroupId)
                .OnDelete(DeleteBehavior.Restrict);

            // RegistrationFormGroup → RegistrationFormFields (one-to-many)
            builder.HasMany(rfg => rfg.FormFields)
                .WithOne(rff => rff.RegistrationFormGroup)
                .HasForeignKey(rff => rff.FormGroupId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}

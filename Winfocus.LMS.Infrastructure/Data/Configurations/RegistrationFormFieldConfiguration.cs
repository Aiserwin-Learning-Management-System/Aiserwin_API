namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// EF Core configuration for <see cref="RegistrationFormField"/>.
    /// Standalone configuration — lightweight junction entity without BaseEntity.
    /// </summary>
    public class RegistrationFormFieldConfiguration : IEntityTypeConfiguration<RegistrationFormField>
    {
        /// <summary>
        /// Configures the entity of type <typeparamref name="TEntity" />.
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity type.</param>
        public void Configure(EntityTypeBuilder<RegistrationFormField> builder)
        {
            // ── Table ────────────────────────────────────────────
            builder.ToTable("RegistrationFormFields");

            // ── Primary Key ──────────────────────────────────────
            builder.HasKey(rff => rff.Id);

            builder.Property(rff => rff.Id)
                .HasColumnType("uniqueidentifier")
                .HasDefaultValueSql("NEWSEQUENTIALID()")
                .ValueGeneratedOnAdd();

            // ── FormId (required FK) ─────────────────────────────
            builder.Property(rff => rff.FormId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            // ── FieldId (required FK) ────────────────────────────
            builder.Property(rff => rff.FieldId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            // ── FormGroupId (nullable FK) ────────────────────────
            builder.Property(rff => rff.FormGroupId)
                .IsRequired(false)
                .HasColumnType("uniqueidentifier");

            // ── DisplayOrder ─────────────────────────────────────
            builder.Property(rff => rff.DisplayOrder)
                .IsRequired()
                .HasDefaultValue(0)
                .HasColumnType("int");

            // ── IsRequired ───────────────────────────────────────
            builder.Property(rff => rff.IsRequired)
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnType("bit");

            // ── Indexes ──────────────────────────────────────────

            // Prevent duplicate fields in same form
            builder.HasIndex(rff => new { rff.FormId, rff.FieldId })
                .IsUnique()
                .HasDatabaseName("IX_RegFormFields_FormId_FieldId_Unique");

            // Ordered fields within a group
            builder.HasIndex(rff => new { rff.FormGroupId, rff.DisplayOrder })
                .HasDatabaseName("IX_RegFormFields_GroupId_DisplayOrder");

            // Ordered fields within a form (for ungrouped)
            builder.HasIndex(rff => new { rff.FormId, rff.DisplayOrder })
                .HasDatabaseName("IX_RegFormFields_FormId_DisplayOrder");

            // ── Relationships ────────────────────────────────────

            // RegistrationFormField → FormField (many-to-one)
            builder.HasOne(rff => rff.FormField)
                .WithMany()
                .HasForeignKey(rff => rff.FieldId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

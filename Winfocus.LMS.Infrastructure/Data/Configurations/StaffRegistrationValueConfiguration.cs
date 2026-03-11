namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// EF Core configuration for <see cref="StaffRegistrationValue"/>.
    /// Standalone configuration — lightweight value-storage entity.
    /// </summary>
    public class StaffRegistrationValueConfiguration : IEntityTypeConfiguration<StaffRegistrationValue>
    {
        /// <summary>
        /// Configures the entity of type <typeparamref name="TEntity" />.
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity type.</param>
        public void Configure(EntityTypeBuilder<StaffRegistrationValue> builder)
        {
            // ── Table ────────────────────────────────────────────
            builder.ToTable("StaffRegistrationValues");

            // ── Primary Key ──────────────────────────────────────
            builder.HasKey(srv => srv.Id);

            builder.Property(srv => srv.Id)
                .HasColumnType("uniqueidentifier")
                .HasDefaultValueSql("NEWSEQUENTIALID()")
                .ValueGeneratedOnAdd();

            // ── RegistrationId (required FK) ─────────────────────
            builder.Property(srv => srv.RegistrationId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            // ── FieldId (required FK) ────────────────────────────
            builder.Property(srv => srv.FieldId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            // ── FieldName (snapshot) ─────────────────────────────
            builder.Property(srv => srv.FieldName)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("nvarchar(100)");

            // ── Value (NVARCHAR(MAX)) ────────────────────────────
            builder.Property(srv => srv.Value)
                .IsRequired(false)
                .HasColumnType("nvarchar(max)");

            // ── Indexes ──────────────────────────────────────────

            // One value per field per registration
            builder.HasIndex(srv => new { srv.RegistrationId, srv.FieldId })
                .IsUnique()
                .HasDatabaseName("IX_StaffRegValues_RegId_FieldId_Unique");

            // Lookup values by field across registrations
            builder.HasIndex(srv => srv.FieldId)
                .HasDatabaseName("IX_StaffRegValues_FieldId");

            // ── Relationships ────────────────────────────────────

            // StaffRegistrationValue → FormField (many-to-one)
            builder.HasOne(srv => srv.FormField)
                .WithMany()
                .HasForeignKey(srv => srv.FieldId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

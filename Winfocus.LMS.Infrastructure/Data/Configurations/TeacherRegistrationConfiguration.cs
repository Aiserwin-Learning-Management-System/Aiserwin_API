namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Domain.Enums;

    /// <summary>
    /// EF Core configuration for <see cref="TeacherRegistration"/>.
    /// </summary>
    public class TeacherRegistrationConfiguration : BaseEntityConfiguration<TeacherRegistration>
    {
        /// <summary>
        /// Override in derived configurations to add entity-specific
        /// columns, indexes, relationships, and constraints.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void ConfigureEntity(EntityTypeBuilder<TeacherRegistration> builder)
        {
            // ── Table ────────────────────────────────────────────
            builder.ToTable("TeacherRegistrations");

            // ── EmployeeId ───────────────────────────────────────
            builder.Property(tr => tr.EmployeeId)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnType("nvarchar(20)");

            // ── EmploymentTypeId ────────────────────────────────
            builder.Property(tr => tr.EmploymentTypeId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            // ── WorkMode (enum → int) ───────────────────────────
            builder.Property(tr => tr.WorkMode)
                .IsRequired()
                .HasConversion<int>()
                .HasColumnType("int");

            // ── Personal Details ────────────────────────────────
            builder.Property(tr => tr.FullName)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("nvarchar(200)");

            builder.Property(tr => tr.EmailAddress)
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnType("nvarchar(150)");

            builder.Property(tr => tr.DateOfBirth)
                .IsRequired()
                .HasColumnType("datetime2");

            builder.Property(tr => tr.MobileNumber)
                .IsRequired()
                .HasMaxLength(15)
                .HasColumnType("nvarchar(15)");

            builder.Property(tr => tr.Address)
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnType("nvarchar(500)");

            builder.Property(tr => tr.Gender)
                .IsRequired()
                .HasConversion<int>()
                .HasColumnType("int");

            // ── Declaration ─────────────────────────────────────
            builder.Property(tr => tr.IsTermsAccepted)
                .IsRequired()
                .HasColumnType("bit");

            // ── DeclarationDate ────────────────────────────────
            builder.Property(tr => tr.DeclarationDate)
                .HasColumnType("datetime2");

            // ── AdministrativeRemarks ──────────────────────────
            builder.Property(tr => tr.AdministrativeRemarks)
                .HasMaxLength(1000)
                .HasColumnType("nvarchar(1000)");

            // ── Indexes ─────────────────────────────────────────
            builder.HasIndex(tr => tr.EmployeeId)
                .IsUnique()
                .HasDatabaseName("IX_TeacherRegistrations_EmployeeId");

            builder.HasIndex(tr => tr.EmailAddress)
                .IsUnique()
                .HasDatabaseName("IX_TeacherRegistrations_EmailAddress");

            builder.HasIndex(tr => tr.EmploymentTypeId)
                .HasDatabaseName("IX_TeacherRegistrations_EmploymentTypeId");

            // ── Relationships ───────────────────────────────────
            builder.HasOne(tr => tr.EmploymentType)
                .WithMany()
                .HasForeignKey(tr => tr.EmploymentTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

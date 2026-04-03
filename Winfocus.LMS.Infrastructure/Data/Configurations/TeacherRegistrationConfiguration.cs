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

            builder.Property(tr => tr.AlternativeMobileNumber)
                .HasMaxLength(15)
                .HasColumnType("nvarchar(15)");

            builder.Property(tr => tr.EmailAddress)
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnType("nvarchar(150)");

            builder.Property(tr => tr.AlternativeEmailAddress)
                .HasMaxLength(150)
                .HasColumnType("nvarchar(150)");

            builder.Property(tr => tr.ResidentialAddress)
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnType("nvarchar(500)");

            builder.Property(tr => tr.Pincode)
                .HasMaxLength(20)
                .HasColumnType("nvarchar(20)");

            builder.Property(tr => tr.DistrictOrLocation)
                .HasMaxLength(200)
                .HasColumnType("nvarchar(200)");

            builder.Property(tr => tr.RefernceContactNumber)
                .HasMaxLength(50)
                .HasColumnType("nvarchar(50)");

            builder.Property(tr => tr.RefernceContactName)
                .HasMaxLength(200)
                .HasColumnType("nvarchar(200)");

            builder.Property(tr => tr.Gender)
                .IsRequired()
                .HasConversion<int>()
                .HasColumnType("int");

            builder.Property(tr => tr.IsWillingToWorkWeekends)
                .IsRequired()
                .HasColumnType("bit");

            builder.Property(tr => tr.HasInternetAndSystemAvailability)
                .IsRequired()
                .HasColumnType("bit");

            // ── Terms/Agreement/Status ──────────────────────────
            builder.Property(tr => tr.IsTermsAccepted)
                .IsRequired()
                .HasColumnType("bit");

            builder.Property(tr => tr.IsDeclared)
                .HasColumnType("bit");

            builder.Property(tr => tr.IsSignedAgreement)
                .HasColumnType("bit");

            builder.Property(tr => tr.Status)
                .IsRequired()
                .HasConversion<int>()
                .HasColumnType("int");

            // ── Date of joining / joining date ──────────────────
            builder.Property(tr => tr.DateOfJoining)
                .HasColumnType("datetime2");

            builder.Property(tr => tr.JoiningDate)
                .HasColumnType("datetime2");

            // ── Contract duration ──────────────────────────────
            builder.Property(tr => tr.ContractDuration)
                .HasColumnType("decimal(18,2)");

            // ── AdministrativeRemarks ──────────────────────────
            builder.Property(tr => tr.AdministrativeRemarks)
                .HasMaxLength(1000)
                .HasColumnType("nvarchar(1000)");

            // ── Location relationships ──────────────────────────
            builder.Property(tr => tr.CountryId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            builder.Property(tr => tr.StateId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            // ── Indexes ─────────────────────────────────────────
            builder.HasIndex(tr => tr.EmployeeId)
                .IsUnique()
                .HasDatabaseName("IX_TeacherRegistrations_EmployeeId");

            builder.HasIndex(tr => tr.EmailAddress)
                .IsUnique()
                .HasDatabaseName("IX_TeacherRegistrations_EmailAddress");

            builder.HasIndex(tr => tr.EmploymentTypeId)
                .HasDatabaseName("IX_TeacherRegistrations_EmploymentTypeId");

            builder.HasIndex(tr => tr.CountryId)
                .HasDatabaseName("IX_TeacherRegistrations_CountryId");

            // ── Relationships ───────────────────────────────────
            builder.HasOne(tr => tr.EmploymentType)
                .WithMany()
                .HasForeignKey(tr => tr.EmploymentTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(tr => tr.Country)
                .WithMany()
                .HasForeignKey(tr => tr.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(tr => tr.State)
                .WithMany()
                .HasForeignKey(tr => tr.StateId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

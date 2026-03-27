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

            builder.Property(tr => tr.MaritalStatus)
                .IsRequired()
                .HasConversion<int>()
                .HasColumnType("int");

            builder.Property(tr => tr.IdProofType)
                .IsRequired()
                .HasConversion<int>()
                .HasColumnType("int");

            builder.Property(tr => tr.IdProofNumber)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("nvarchar(50)");

            builder.Property(tr => tr.ComputerLiteracy)
                .IsRequired()
                .HasConversion<int>()
                .HasColumnType("int");

            // ── Academic Details ────────────────────────────────
            builder.Property(tr => tr.HighestQualification)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("nvarchar(200)");

            // ── Professional Details ────────────────────────────
            builder.Property(tr => tr.SalaryStructure)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("nvarchar(200)");

            builder.Property(tr => tr.PaymentCycle)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("nvarchar(100)");

            builder.Property(tr => tr.ContractDuration)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("nvarchar(100)");

            builder.Property(tr => tr.ReportingManager)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("nvarchar(200)");

            // ── File Paths ──────────────────────────────────────
            builder.Property(tr => tr.PhotoPath)
                .IsRequired(false)
                .HasMaxLength(500)
                .HasColumnType("nvarchar(500)");

            builder.Property(tr => tr.IdCardPath)
                .IsRequired(false)
                .HasMaxLength(500)
                .HasColumnType("nvarchar(500)");

            // ── Declaration ─────────────────────────────────────
            builder.Property(tr => tr.IsTermsAccepted)
                .IsRequired()
                .HasColumnType("bit");

            builder.Property(tr => tr.IsDeclarationAccepted)
                .IsRequired()
                .HasColumnType("bit");

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

            builder.HasMany(tr => tr.PreferredGrades)
                .WithOne(tg => tg.TeacherRegistration)
                .HasForeignKey(tg => tg.TeacherRegistrationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(tr => tr.PreferredSubjects)
                .WithOne(ts => ts.TeacherRegistration)
                .HasForeignKey(ts => ts.TeacherRegistrationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(tr => tr.BoardsHandled)
                .WithOne(ts => ts.TeacherRegistration)
                .HasForeignKey(ts => ts.TeacherRegistrationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(tr => tr.GradesTaughtEarlier)
                .WithOne(tg => tg.TeacherRegistration)
                .HasForeignKey(tg => tg.TeacherRegistrationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(tr => tr.SubjectsTaughtEarlier)
                .WithOne(ts => ts.TeacherRegistration)
                .HasForeignKey(ts => ts.TeacherRegistrationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(tr => tr.ToolsKnown)
                .WithOne(tt => tt.TeacherRegistration)
                .HasForeignKey(tt => tt.TeacherRegistrationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(tr => tr.LanguagesKnown)
                .WithOne(tl => tl.TeacherRegistration)
                .HasForeignKey(tl => tl.TeacherRegistrationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
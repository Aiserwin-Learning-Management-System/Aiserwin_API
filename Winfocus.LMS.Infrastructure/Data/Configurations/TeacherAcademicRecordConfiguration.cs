namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// EF Core configuration for <see cref="TeacherAcademicRecord"/>.
    /// </summary>
    public class TeacherAcademicRecordConfiguration : BaseEntityConfiguration<TeacherAcademicRecord>
    {
        /// <summary>
        /// Override in derived configurations to add entity-specific
        /// columns, indexes, relationships, and constraints.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void ConfigureEntity(EntityTypeBuilder<TeacherAcademicRecord> builder)
        {
            // ── Table ────────────────────────────────────────────
            builder.ToTable("TeacherAcademicRecords");

            // ── TeacherRegistrationId ───────────────────────────
            builder.Property(tar => tar.TeacherRegistrationId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            // ── CourseName ───────────────────────────────────────
            builder.Property(tar => tar.CourseName)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("nvarchar(200)");

            // ── MarksPercentage ──────────────────────────────────
            builder.Property(tar => tar.MarksPercentage)
                .IsRequired()
                .HasColumnType("decimal(5,2)");

            // ── Subjects ─────────────────────────────────────────
            builder.Property(tar => tar.Subjects)
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnType("nvarchar(500)");

        }
    }
}

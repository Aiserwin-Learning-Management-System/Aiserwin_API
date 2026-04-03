namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// EF Core configuration for <see cref="TeacherSyllabus"/>.
    /// </summary>
    public class TeacherSyllabusConfiguration : BaseEntityConfiguration<TeacherSyllabus>
    {
        /// <summary>
        /// Override in derived configurations to add entity-specific
        /// columns, indexes, relationships, and constraints.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void ConfigureEntity(EntityTypeBuilder<TeacherSyllabus> builder)
        {
            // ── Table ────────────────────────────────────────────
            builder.ToTable("TeacherSyllabi");

            // ── TeacherRegistrationId ───────────────────────────
            builder.Property(ts => ts.TeacherRegistrationId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            // ── ExamSyllabusId ──────────────────────────────────
            builder.Property(ts => ts.SyllabusId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            // ── Indexes ─────────────────────────────────────────
            builder.HasIndex(ts => new { ts.TeacherRegistrationId, ts.SyllabusId })
                .IsUnique()
                .HasDatabaseName("IX_TeacherSyllabi_TeacherRegistrationId_ExamSyllabusId");

            builder.HasOne(ts => ts.Syllabus)
                .WithMany()
                .HasForeignKey(ts => ts.SyllabusId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

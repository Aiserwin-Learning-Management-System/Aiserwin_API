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
            builder.Property(ts => ts.ExamSyllabusId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            // ── Indexes ─────────────────────────────────────────
            builder.HasIndex(ts => new { ts.TeacherRegistrationId, ts.ExamSyllabusId })
                .IsUnique()
                .HasDatabaseName("IX_TeacherSyllabi_TeacherRegistrationId_ExamSyllabusId");

            // ── Relationships ───────────────────────────────────
            builder.HasOne(ts => ts.TeacherRegistration)
                .WithMany(tr => tr.BoardsHandled)
                .HasForeignKey(ts => ts.TeacherRegistrationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ts => ts.ExamSyllabus)
                .WithMany()
                .HasForeignKey(ts => ts.ExamSyllabusId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
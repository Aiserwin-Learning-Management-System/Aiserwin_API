namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// EF Core configuration for <see cref="TeacherTaughtSubject"/>.
    /// </summary>
    public class TeacherTaughtSubjectConfiguration : BaseEntityConfiguration<TeacherTaughtSubject>
    {
        /// <summary>
        /// Override in derived configurations to add entity-specific
        /// columns, indexes, relationships, and constraints.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void ConfigureEntity(EntityTypeBuilder<TeacherTaughtSubject> builder)
        {
            // ── Table ────────────────────────────────────────────
            builder.ToTable("TeacherTaughtSubjects");

            // ── TeacherRegistrationId ───────────────────────────
            builder.Property(tts => tts.TeacherRegistrationId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            // ── ExamSubjectId ───────────────────────────────────
            builder.Property(tts => tts.SubjectId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            // ── Indexes ─────────────────────────────────────────
            builder.HasIndex(tts => new { tts.TeacherRegistrationId, tts.SubjectId })
                .IsUnique()
                .HasDatabaseName("IX_TeacherTaughtSubjects_TeacherRegistrationId_ExamSubjectId");

            builder.HasOne(tts => tts.Subject)
                .WithMany()
                .HasForeignKey(tts => tts.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

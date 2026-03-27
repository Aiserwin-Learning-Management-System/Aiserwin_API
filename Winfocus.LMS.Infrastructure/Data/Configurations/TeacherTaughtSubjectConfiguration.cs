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
            builder.Property(tts => tts.ExamSubjectId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            // ── Indexes ─────────────────────────────────────────
            builder.HasIndex(tts => new { tts.TeacherRegistrationId, tts.ExamSubjectId })
                .IsUnique()
                .HasDatabaseName("IX_TeacherTaughtSubjects_TeacherRegistrationId_ExamSubjectId");

            // ── Relationships ───────────────────────────────────
            builder.HasOne(tts => tts.TeacherRegistration)
                .WithMany(tr => tr.SubjectsTaughtEarlier)
                .HasForeignKey(tts => tts.TeacherRegistrationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(tts => tts.ExamSubject)
                .WithMany()
                .HasForeignKey(tts => tts.ExamSubjectId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// EF Core configuration for <see cref="TeacherPreferredSubject"/>.
    /// </summary>
    public class TeacherPreferredSubjectConfiguration : BaseEntityConfiguration<TeacherPreferredSubject>
    {
        /// <summary>
        /// Override in derived configurations to add entity-specific
        /// columns, indexes, relationships, and constraints.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void ConfigureEntity(EntityTypeBuilder<TeacherPreferredSubject> builder)
        {
            // ── Table ────────────────────────────────────────────
            builder.ToTable("TeacherPreferredSubjects");

            // ── TeacherRegistrationId ───────────────────────────
            builder.Property(tps => tps.TeacherRegistrationId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            // ── ExamSubjectId ───────────────────────────────────
            builder.Property(tps => tps.ExamSubjectId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            // ── Indexes ─────────────────────────────────────────
            builder.HasIndex(tps => new { tps.TeacherRegistrationId, tps.ExamSubjectId })
                .IsUnique()
                .HasDatabaseName("IX_TeacherPreferredSubjects_TeacherRegistrationId_ExamSubjectId");

            // ── Relationships ───────────────────────────────────
            builder.HasOne(tps => tps.TeacherRegistration)
                .WithMany(tr => tr.PreferredSubjects)
                .HasForeignKey(tps => tps.TeacherRegistrationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(tps => tps.ExamSubject)
                .WithMany()
                .HasForeignKey(tps => tps.ExamSubjectId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// EF Core configuration for <see cref="TeacherTaughtGrade"/>.
    /// </summary>
    public class TeacherTaughtGradeConfiguration : BaseEntityConfiguration<TeacherTaughtGrade>
    {
        /// <summary>
        /// Override in derived configurations to add entity-specific
        /// columns, indexes, relationships, and constraints.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void ConfigureEntity(EntityTypeBuilder<TeacherTaughtGrade> builder)
        {
            // ── Table ────────────────────────────────────────────
            builder.ToTable("TeacherTaughtGrades");

            // ── TeacherRegistrationId ───────────────────────────
            builder.Property(ttg => ttg.TeacherRegistrationId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            // ── ExamGradeId ─────────────────────────────────────
            builder.Property(ttg => ttg.ExamGradeId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            // ── Indexes ─────────────────────────────────────────
            builder.HasIndex(ttg => new { ttg.TeacherRegistrationId, ttg.ExamGradeId })
                .IsUnique()
                .HasDatabaseName("IX_TeacherTaughtGrades_TeacherRegistrationId_ExamGradeId");

            // ── Relationships ───────────────────────────────────
            builder.HasOne(ttg => ttg.TeacherRegistration)
                .WithMany(tr => tr.GradesTaughtEarlier)
                .HasForeignKey(ttg => ttg.TeacherRegistrationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ttg => ttg.ExamGrade)
                .WithMany()
                .HasForeignKey(ttg => ttg.ExamGradeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
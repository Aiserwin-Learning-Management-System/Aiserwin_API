namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// EF Core configuration for <see cref="TeacherPreferredGrade"/>.
    /// </summary>
    public class TeacherPreferredGradeConfiguration : BaseEntityConfiguration<TeacherPreferredGrade>
    {
        /// <summary>
        /// Override in derived configurations to add entity-specific
        /// columns, indexes, relationships, and constraints.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void ConfigureEntity(EntityTypeBuilder<TeacherPreferredGrade> builder)
        {
            // ── Table ────────────────────────────────────────────
            builder.ToTable("TeacherPreferredGrades");

            // ── TeacherRegistrationId ───────────────────────────
            builder.Property(tpg => tpg.TeacherRegistrationId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            // ── ExamGradeId ─────────────────────────────────────
            builder.Property(tpg => tpg.ExamGradeId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            // ── Indexes ─────────────────────────────────────────
            builder.HasIndex(tpg => new { tpg.TeacherRegistrationId, tpg.ExamGradeId })
                .IsUnique()
                .HasDatabaseName("IX_TeacherPreferredGrades_TeacherRegistrationId_ExamGradeId");

            builder.HasOne(tpg => tpg.ExamGrade)
                .WithMany()
                .HasForeignKey(tpg => tpg.ExamGradeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

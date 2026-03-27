namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Domain.Enums;

    /// <summary>
    /// EF Core configuration for <see cref="TeacherGrade"/>.
    /// </summary>
    public class TeacherGradeConfiguration : BaseEntityConfiguration<TeacherGrade>
    {
        /// <summary>
        /// Override in derived configurations to add entity-specific
        /// columns, indexes, relationships, and constraints.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void ConfigureEntity(EntityTypeBuilder<TeacherGrade> builder)
        {
            // ── Table ────────────────────────────────────────────
            builder.ToTable("TeacherGrades");

            // ── TeacherRegistrationId ───────────────────────────
            builder.Property(tg => tg.TeacherRegistrationId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            // ── ExamGradeId ─────────────────────────────────────
            builder.Property(tg => tg.ExamGradeId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            // ── Type (enum → int) ───────────────────────────────
            builder.Property(tg => tg.Type)
                .IsRequired()
                .HasConversion<int>()
                .HasColumnType("int");

            // ── Indexes ─────────────────────────────────────────
            builder.HasIndex(tg => new { tg.TeacherRegistrationId, tg.ExamGradeId, tg.Type })
                .IsUnique()
                .HasDatabaseName("IX_TeacherGrades_TeacherRegistrationId_ExamGradeId_Type");

            // ── Relationships ───────────────────────────────────
            builder.HasOne(tg => tg.TeacherRegistration)
                .WithMany(tr => tr.PreferredGrades)  // Note: This assumes PreferredGrades includes both types, adjust if needed
                .HasForeignKey(tg => tg.TeacherRegistrationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(tg => tg.ExamGrade)
                .WithMany()
                .HasForeignKey(tg => tg.ExamGradeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
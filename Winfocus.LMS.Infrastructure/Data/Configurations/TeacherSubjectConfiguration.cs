namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Domain.Enums;

    /// <summary>
    /// EF Core configuration for <see cref="TeacherSubject"/>.
    /// </summary>
    public class TeacherSubjectConfiguration : BaseEntityConfiguration<TeacherSubject>
    {
        /// <summary>
        /// Override in derived configurations to add entity-specific
        /// columns, indexes, relationships, and constraints.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void ConfigureEntity(EntityTypeBuilder<TeacherSubject> builder)
        {
            // ── Table ────────────────────────────────────────────
            builder.ToTable("TeacherSubjects");

            // ── TeacherRegistrationId ───────────────────────────
            builder.Property(ts => ts.TeacherRegistrationId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            // ── ExamSubjectId ───────────────────────────────────
            builder.Property(ts => ts.ExamSubjectId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            // ── Type (enum → int) ───────────────────────────────
            builder.Property(ts => ts.Type)
                .IsRequired()
                .HasConversion<int>()
                .HasColumnType("int");

            // ── Indexes ─────────────────────────────────────────
            builder.HasIndex(ts => new { ts.TeacherRegistrationId, ts.ExamSubjectId, ts.Type })
                .IsUnique()
                .HasDatabaseName("IX_TeacherSubjects_TeacherRegistrationId_ExamSubjectId_Type");

            // ── Relationships ───────────────────────────────────
            builder.HasOne(ts => ts.TeacherRegistration)
                .WithMany(tr => tr.PreferredSubjects)
                .HasForeignKey(ts => ts.TeacherRegistrationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ts => ts.ExamSubject)
                .WithMany()
                .HasForeignKey(ts => ts.ExamSubjectId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
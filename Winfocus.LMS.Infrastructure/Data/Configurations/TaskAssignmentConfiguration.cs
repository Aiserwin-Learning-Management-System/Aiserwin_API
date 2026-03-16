namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// TaskAssignmentConfiguration.
    /// </summary>
    /// <seealso cref="Winfocus.LMS.Infrastructure.Data.Configurations.BaseEntityConfiguration&lt;Winfocus.LMS.Domain.Entities.TaskAssignment&gt;" />
    public class TaskAssignmentConfiguration : BaseEntityConfiguration<TaskAssignment>
    {
        /// <summary>
        /// Override in derived configurations to add entity-specific
        /// columns, indexes, relationships, and constraints.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void ConfigureEntity(EntityTypeBuilder<TaskAssignment> builder)
        {
            builder.ToTable("TaskAssignments");

            // ── Operator FK ──────────────────────────────────────
            builder.Property(t => t.OperatorId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            // ── AssignedBy (Guid — user ID) ──────────────────────
            builder.Property(t => t.AssignedBy)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            // ── ResourceTypeId FK (required) ─────────────────────
            builder.Property(t => t.ResourceTypeId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            // ── QuestionType (int) ───────────────────────────────
            builder.Property(t => t.QuestionType)
                .IsRequired()
                .HasColumnType("int");

            // ── Year (optional) ──────────────────────────────────
            builder.Property(t => t.Year)
                .IsRequired(false)
                .HasColumnType("int");

            // ── Master FKs (required) ────────────────────────────
            builder.Property(t => t.SyllabusId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            builder.Property(t => t.GradeId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            builder.Property(t => t.SubjectId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            // ── Master FKs (nullable) ────────────────────────────
            builder.Property(t => t.UnitId)
                .IsRequired(false)
                .HasColumnType("uniqueidentifier");

            builder.Property(t => t.ChapterId)
                .IsRequired(false)
                .HasColumnType("uniqueidentifier");

            // ── Task Details ─────────────────────────────────────
            builder.Property(t => t.TotalQuestions)
                .IsRequired()
                .HasColumnType("int");

            builder.Property(t => t.CompletedCount)
                .IsRequired()
                .HasDefaultValue(0)
                .HasColumnType("int");

            builder.Property(t => t.Deadline)
                .IsRequired()
                .HasColumnType("datetime2");

            builder.Property(t => t.Priority)
                .IsRequired()
                .HasDefaultValue(1) // Medium
                .HasColumnType("int");

            builder.Property(t => t.Instructions)
                .IsRequired(false)
                .HasMaxLength(2000)
                .HasColumnType("nvarchar(2000)");

            builder.Property(t => t.Status)
                .IsRequired()
                .HasDefaultValue(0) // Pending
                .HasColumnType("int");

            // ── Indexes ──────────────────────────────────────────
            builder.HasIndex(t => new { t.OperatorId, t.Status })
                .HasDatabaseName("IX_TaskAssignments_OperatorId_Status");

            builder.HasIndex(t => t.Deadline)
                .HasDatabaseName("IX_TaskAssignments_Deadline");

            builder.HasIndex(t => t.SyllabusId)
                .HasDatabaseName("IX_TaskAssignments_SyllabusId");

            builder.HasIndex(t => t.SubjectId)
                .HasDatabaseName("IX_TaskAssignments_SubjectId");

            // ── Relationships ────────────────────────────────────

            // → StaffRegistration (operator)
            builder.HasOne(t => t.Operator)
                .WithMany()
                .HasForeignKey(t => t.OperatorId)
                .OnDelete(DeleteBehavior.Restrict);

            // → ContentResourceType
            builder.HasOne(t => t.ResourceType)
                .WithMany()
                .HasForeignKey(t => t.ResourceTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // → ExamSyllabus
            builder.HasOne(t => t.Syllabus)
                .WithMany()
                .HasForeignKey(t => t.SyllabusId)
                .OnDelete(DeleteBehavior.Restrict);

            // → ExamGrade
            builder.HasOne(t => t.Grade)
                .WithMany()
                .HasForeignKey(t => t.GradeId)
                .OnDelete(DeleteBehavior.NoAction);

            // → ExamSubject
            builder.HasOne(t => t.Subject)
                .WithMany()
                .HasForeignKey(t => t.SubjectId)
                .OnDelete(DeleteBehavior.NoAction);

            // → ExamUnit (nullable)
            builder.HasOne(t => t.Unit)
                .WithMany()
                .HasForeignKey(t => t.UnitId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            // → ExamChapter (nullable)
            builder.HasOne(t => t.Chapter)
                .WithMany()
                .HasForeignKey(t => t.ChapterId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            // → Questions
            builder.HasMany(t => t.Questions)
                .WithOne(q => q.TaskAssignment)
                .HasForeignKey(q => q.TaskId)
                .OnDelete(DeleteBehavior.Restrict);

            // → DailyActivityReports
            builder.HasMany(t => t.DailyActivityReports)
                .WithOne(d => d.TaskAssignment)
                .HasForeignKey(d => d.TaskId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}

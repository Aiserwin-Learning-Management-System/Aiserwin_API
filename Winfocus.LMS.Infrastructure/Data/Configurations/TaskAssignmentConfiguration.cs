namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Domain.Enums;
    using Winfocus.LMS.Infrastructure.Data.Configurations;

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

            // ── OperatorId ───────────────────────────────────────
            builder.Property(t => t.OperatorId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            // ── AssignedBy (business field, separate from CreatedBy) ──
            builder.Property(t => t.AssignedBy)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("nvarchar(100)");

            // ── QuestionType ─────────────────────────────────────
            builder.Property(t => t.QuestionType)
                .IsRequired()
                .HasConversion<int>()
                .HasColumnType("int");

            // ── Academic context ─────────────────────────────────
            builder.Property(t => t.Year).IsRequired(false).HasColumnType("int");
            builder.Property(t => t.Syllabus).IsRequired(false).HasMaxLength(100).HasColumnType("nvarchar(100)");
            builder.Property(t => t.Grade).IsRequired(false).HasMaxLength(50).HasColumnType("nvarchar(50)");
            builder.Property(t => t.Stream).IsRequired(false).HasMaxLength(100).HasColumnType("nvarchar(100)");
            builder.Property(t => t.Course).IsRequired(false).HasMaxLength(100).HasColumnType("nvarchar(100)");
            builder.Property(t => t.Subject).IsRequired(false).HasMaxLength(100).HasColumnType("nvarchar(100)");
            builder.Property(t => t.Chapter).IsRequired(false).HasMaxLength(200).HasColumnType("nvarchar(200)");

            // ── Progress ─────────────────────────────────────────
            builder.Property(t => t.TotalQuestions).IsRequired().HasColumnType("int");
            builder.Property(t => t.CompletedCount).IsRequired().HasDefaultValue(0).HasColumnType("int");

            // ── Deadline & Priority ──────────────────────────────
            builder.Property(t => t.Deadline).IsRequired().HasColumnType("datetime2");
            builder.Property(t => t.Priority)
                .IsRequired()
                .HasConversion<int>()
                .HasDefaultValue(TaskPriority.Medium)
                .HasColumnType("int");

            // ── Instructions ─────────────────────────────────────
            builder.Property(t => t.Instructions)
                .IsRequired(false)
                .HasMaxLength(2000)
                .HasColumnType("nvarchar(2000)");

            // ── Status ───────────────────────────────────────────
            builder.Property(t => t.Status)
                .IsRequired()
                .HasConversion<int>()
                .HasDefaultValue(Domain.Enums.TaskStatus.Pending)
                .HasColumnType("int");

            // ── Indexes ──────────────────────────────────────────
            builder.HasIndex(t => new { t.OperatorId, t.Status })
                .HasDatabaseName("IX_TaskAssignments_OperatorId_Status");

            builder.HasIndex(t => t.Deadline)
                .HasDatabaseName("IX_TaskAssignments_Deadline");

            builder.HasIndex(t => t.Priority)
                .HasDatabaseName("IX_TaskAssignments_Priority");

            // ── Relationships ────────────────────────────────────
            builder.HasOne(t => t.Operator)
                .WithMany()
                .HasForeignKey(t => t.OperatorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(t => t.Questions)
                .WithOne(q => q.TaskAssignment)
                .HasForeignKey(q => q.TaskId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(t => t.DailyReports)
                .WithOne(d => d.TaskAssignment)
                .HasForeignKey(d => d.TaskId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}

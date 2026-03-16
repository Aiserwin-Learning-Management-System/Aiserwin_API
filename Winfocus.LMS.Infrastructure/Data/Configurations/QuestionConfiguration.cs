namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Domain.Enums;

    /// <summary>
    /// QuestionConfiguration.
    /// </summary>
    /// <seealso cref="Winfocus.LMS.Infrastructure.Data.Configurations.BaseEntityConfiguration&lt;Winfocus.LMS.Domain.Entities.Question&gt;" />
    public class QuestionConfiguration : BaseEntityConfiguration<Question>
    {
        /// <summary>
        /// Override in derived configurations to add entity-specific
        /// columns, indexes, relationships, and constraints.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void ConfigureEntity(EntityTypeBuilder<Question> builder)
        {
            builder.ToTable("Questions");

            builder.Property(q => q.TaskId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            builder.Property(q => q.OperatorId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            builder.Property(q => q.QuestionType)
                .IsRequired()
                .HasColumnType("int");

            builder.Property(q => q.QuestionText)
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            builder.Property(q => q.Marks)
                .IsRequired(false)
                .HasColumnType("decimal(5,2)");

            builder.Property(q => q.CorrectAnswer)
                .IsRequired(false)
                .HasMaxLength(10)
                .HasColumnType("nvarchar(10)");

            builder.Property(q => q.CorrectAnswerText)
                .IsRequired(false)
                .HasColumnType("nvarchar(max)");

            builder.Property(q => q.Reference)
                .IsRequired(false)
                .HasMaxLength(500)
                .HasColumnType("nvarchar(500)");

            builder.Property(q => q.Status)
                   .IsRequired()
                    .HasConversion<int>()
                    .HasDefaultValue(QuestionStatus.Draft)
                    .HasColumnType("int");

            // ── Indexes ──────────────────────────────────────────
            builder.HasIndex(q => new { q.TaskId, q.Status })
                .HasDatabaseName("IX_Questions_TaskId_Status");

            builder.HasIndex(q => new { q.OperatorId, q.Status })
                .HasDatabaseName("IX_Questions_OperatorId_Status");

            // ── Relationships ────────────────────────────────────

            // → StaffRegistration (operator)
            builder.HasOne(q => q.Operator)
                .WithMany()
                .HasForeignKey(q => q.OperatorId)
                .OnDelete(DeleteBehavior.Restrict);

            // → QuestionOptions (CASCADE DELETE)
            builder.HasMany(q => q.Options)
                .WithOne(o => o.Question)
                .HasForeignKey(o => o.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            // → QuestionReviews
            builder.HasMany(q => q.Reviews)
                .WithOne(r => r.Question)
                .HasForeignKey(r => r.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

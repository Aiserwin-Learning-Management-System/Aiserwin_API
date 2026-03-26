namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;

    public class ExamQuestionConfiguration : IEntityTypeConfiguration<ExamQuestion>
    {
        public void Configure(EntityTypeBuilder<ExamQuestion> builder)
        {
            builder.ToTable("ExamQuestions");

            builder.Property(e => e.QuestionId).IsRequired().HasColumnType("uniqueidentifier");
            builder.Property(e => e.ExamId).IsRequired().HasColumnType("uniqueidentifier");

            // Indexes on FK columns
            builder.HasIndex(e => e.ExamId).HasDatabaseName("IX_ExamQuestions_ExamId");
            builder.HasIndex(e => e.QuestionId).HasDatabaseName("IX_ExamQuestions_QuestionId");

            // Composite unique index to prevent duplicate question entries in same exam
            builder.HasIndex(e => new { e.ExamId, e.QuestionId })
                .IsUnique()
                .HasDatabaseName("IX_ExamQuestions_ExamId_QuestionId_UQ");

            // Relationships
            builder.HasOne(eq => eq.Exam)
                .WithMany()
                .HasForeignKey(eq => eq.ExamId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(eq => eq.Question)
                .WithMany()
                .HasForeignKey(eq => eq.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

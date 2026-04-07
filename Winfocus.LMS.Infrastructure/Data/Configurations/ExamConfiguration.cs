namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Configures the Exam entity schema and relationships.
    /// </summary>
    public class ExamConfiguration : IEntityTypeConfiguration<Exam>
    {
        /// <summary>
        /// Configures the entity using the specified builder.
        /// </summary>
        /// <param name="builder">Entity type builder for Exam.</param>
        public void Configure(EntityTypeBuilder<Exam> builder)
        {
            builder.ToTable("Exams");

            builder.Property(e => e.CountryId).IsRequired().HasColumnType("uniqueidentifier");
            builder.Property(e => e.CenterId).IsRequired().HasColumnType("uniqueidentifier");
            builder.Property(e => e.SyllabusId).IsRequired().HasColumnType("uniqueidentifier");
            builder.Property(e => e.GradeId).IsRequired().HasColumnType("uniqueidentifier");
            builder.Property(e => e.StreamId).IsRequired().HasColumnType("uniqueidentifier");
            builder.Property(e => e.CourseId).IsRequired().HasColumnType("uniqueidentifier");
            builder.Property(e => e.UnitId).IsRequired().HasColumnType("uniqueidentifier");
            builder.Property(e => e.ChapterId).IsRequired().HasColumnType("uniqueidentifier");
            builder.Property(e => e.QuestionTypeId).IsRequired().HasColumnType("uniqueidentifier");

            builder.Property(e => e.ExamTitle).IsRequired(false).HasColumnType("nvarchar(max)");
            builder.Property(e => e.ExamQuestionNumber).IsRequired(false).HasColumnType("nvarchar(max)");
            builder.Property(e => e.ExamDate).IsRequired().HasColumnType("datetime2");
            builder.Property(e => e.ExamDuration).IsRequired(false).HasColumnType("nvarchar(max)");
            builder.Property(e => e.TotalMark).IsRequired().HasColumnType("float");
            builder.Property(e => e.PassingMark).IsRequired().HasColumnType("float");
            builder.Property(e => e.Status).IsRequired().HasColumnType("int");

            // Indexes on frequently queried columns and FK columns
            builder.HasIndex(e => e.CenterId).HasDatabaseName("IX_Exams_CenterId");
            builder.HasIndex(e => e.ChapterId).HasDatabaseName("IX_Exams_ChapterId");
            builder.HasIndex(e => e.CountryId).HasDatabaseName("IX_Exams_CountryId");
            builder.HasIndex(e => e.CourseId).HasDatabaseName("IX_Exams_CourseId");
            builder.HasIndex(e => e.GradeId).HasDatabaseName("IX_Exams_GradeId");
            builder.HasIndex(e => e.QuestionTypeId).HasDatabaseName("IX_Exams_QuestionTypeId");
            builder.HasIndex(e => e.StreamId).HasDatabaseName("IX_Exams_StreamId");
            builder.HasIndex(e => e.SyllabusId).HasDatabaseName("IX_Exams_SyllabusId");
            builder.HasIndex(e => e.UnitId).HasDatabaseName("IX_Exams_UnitId");

            // Also index ExamDate to speed date range queries
            builder.HasIndex(e => e.ExamDate).HasDatabaseName("IX_Exams_ExamDate");

            // Relationships - match expected delete behaviors
            builder.HasOne(e => e.Center)
                .WithMany()
                .HasForeignKey(e => e.CenterId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Country)
                .WithMany()
                .HasForeignKey(e => e.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Course)
                .WithMany()
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Chapter)
                .WithMany()
                .HasForeignKey(e => e.ChapterId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Unit)
                .WithMany()
                .HasForeignKey(e => e.UnitId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Grade)
                .WithMany()
                .HasForeignKey(e => e.GradeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.QuestionType)
                .WithMany()
                .HasForeignKey(e => e.QuestionTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Stream)
                .WithMany()
                .HasForeignKey(e => e.StreamId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Syllabus)
                .WithMany()
                .HasForeignKey(e => e.SyllabusId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.ModeOfStudy)
                .WithMany()
                .HasForeignKey(e => e.ModeOfStudyId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(e => e.State)
                .WithMany()
                .HasForeignKey(e => e.StateId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(e => e.Subject)
                .WithMany()
                .HasForeignKey(e => e.SubjectId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}

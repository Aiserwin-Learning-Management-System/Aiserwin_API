namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Data.Configurations;

    /// <summary>
    /// Configures the ExamAccount entity schema and relationships.
    /// </summary>
    public class ExamAccountConfiguration : BaseEntityConfiguration<ExamAccount>
    {
        /// <summary>
        /// Override in derived configurations to add entity-specific
        /// columns, indexes, relationships, and constraints.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void ConfigureEntity(EntityTypeBuilder<ExamAccount> builder)
        {
            builder.ToTable("ExamAccounts");

            builder.Property(e => e.ActivationStartDate)
                .IsRequired()
                .HasColumnType("datetime2");

            builder.Property(e => e.ActivationEndDate)
                .IsRequired()
                .HasColumnType("datetime2");

            builder.Property(e => e.ExamDate)
                .IsRequired()
                .HasColumnType("datetime2");

            builder.Property(e => e.ResourceId)
                .IsRequired();

            builder.Property(e => e.QuestionTypeId)
                .IsRequired();

            // Indexes on frequently queried FK columns
            builder.HasIndex(e => e.ExamId).HasDatabaseName("IX_ExamAccounts_ExamId");
            builder.HasIndex(e => e.ChapterId).HasDatabaseName("IX_ExamAccounts_ChapterId");
            builder.HasIndex(e => e.UnitId).HasDatabaseName("IX_ExamAccounts_UnitId");
            builder.HasIndex(e => e.SubjectId).HasDatabaseName("IX_ExamAccounts_SubjectId");
            builder.HasIndex(e => e.BatchId).HasDatabaseName("IX_ExamAccounts_BatchId");
            builder.HasIndex(e => e.StudentId).HasDatabaseName("IX_ExamAccounts_StudentId");
            builder.HasIndex(e => e.QuestionTypeConfigId).HasDatabaseName("IX_ExamAccounts_QuestionTypeConfigId");
            builder.HasIndex(e => e.ResourceTypeId).HasDatabaseName("IX_ExamAccounts_ResourceTypeId");

            // Relationships

            // ExamAccount → Exam: NO ACTION to avoid multiple cascade paths.
            // The cascade chain Exam → ExamChapter (Cascade) + ExamAccount → ExamChapter (NoAction)
            // would conflict if ExamAccount → Exam were also Cascade.
            builder.HasOne(e => e.Exam)
                .WithMany()
                .HasForeignKey(e => e.ExamId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            // ExamAccount → ExamChapter: NO ACTION (required end with global query filter)
            builder.HasOne(e => e.Chapter)
                .WithMany()
                .HasForeignKey(e => e.ChapterId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            // ExamAccount → ExamUnit: NoAction (required to avoid SQL Server cascade cycle).
            // Chain: Subject → ExamUnit (Cascade) + ExamUnit → ExamAccount (Cascade) +
            // ExamAccount → Subject (Cascade) creates Subject → ExamUnit → ExamAccount → Subject cycle.
            builder.HasOne(e => e.Unit)
                .WithMany()
                .HasForeignKey(e => e.UnitId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            // ExamAccount → Subject: Cascade
            builder.HasOne(e => e.Subject)
                .WithMany()
                .HasForeignKey(e => e.SubjectId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            // ExamAccount → ContentResourceType: Cascade
            builder.HasOne(e => e.ResourceType)
                .WithMany()
                .HasForeignKey(e => e.ResourceTypeId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            // ExamAccount → QuestionTypeConfig: Cascade
            builder.HasOne(e => e.QuestionTypeConfig)
                .WithMany()
                .HasForeignKey(e => e.QuestionTypeConfigId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            // ExamAccount → Batch: NoAction (optional relationship)
            builder.HasOne(e => e.Batch)
                .WithMany()
                .HasForeignKey(e => e.BatchId)
                .OnDelete(DeleteBehavior.NoAction);

            // ExamAccount → Student: NoAction (optional relationship)
            builder.HasOne(e => e.Student)
                .WithMany()
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}

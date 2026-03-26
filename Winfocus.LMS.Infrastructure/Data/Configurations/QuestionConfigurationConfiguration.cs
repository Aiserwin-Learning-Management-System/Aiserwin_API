namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Entity Framework Core configuration for the <see cref="QuestionConfig"/> entity.
    /// Configures table name, column constraints, indexes, foreign keys, and query filters.
    /// </summary>
    public class QuestionConfigurationConfiguration : IEntityTypeConfiguration<QuestionConfiguration>
    {
        /// <summary>
        /// Configures the <see cref="QuestionConfig"/> entity mapping.
        /// </summary>
        /// <param name="builder">The entity type builder.</param>
        public void Configure(EntityTypeBuilder<QuestionConfiguration> builder)
        {
            builder.ToTable("QuestionConfigurations");

            builder.Property(x => x.QuestionCode)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.SequenceNumber)
                .IsRequired();

            // Unique Question Code — prevents any duplicate at DB level.
            builder.HasIndex(x => x.QuestionCode)
                .IsUnique()
                .HasFilter("IsDeleted = 0");

            // Composite index for sequence number lookup performance.
            builder.HasIndex(x => new
            {
                x.SyllabusId,
                x.AcademicYearId,
                x.GradeId,
                x.SubjectId,
                x.UnitId,
                x.ChapterId,
                x.QuestionTypeId,
            });

            // FK: Syllabus.
            builder.HasOne(x => x.Syllabus)
                .WithMany()
                .HasForeignKey(x => x.SyllabusId)
                .OnDelete(DeleteBehavior.Restrict);

            // FK: AcademicYear (existing table).
            builder.HasOne(x => x.AcademicYear)
                .WithMany()
                .HasForeignKey(x => x.AcademicYearId)
                .OnDelete(DeleteBehavior.Restrict);

            // FK: Grade.
            builder.HasOne(x => x.Grade)
                .WithMany()
                .HasForeignKey(x => x.GradeId)
                .OnDelete(DeleteBehavior.Restrict);

            // FK: Subject.
            builder.HasOne(x => x.Subject)
                .WithMany()
                .HasForeignKey(x => x.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            // FK: Unit.
            builder.HasOne(x => x.Unit)
                .WithMany()
                .HasForeignKey(x => x.UnitId)
                .OnDelete(DeleteBehavior.Restrict);

            // FK: Chapter.
            builder.HasOne(x => x.Chapter)
                .WithMany()
                .HasForeignKey(x => x.ChapterId)
                .OnDelete(DeleteBehavior.Restrict);

            // FK: ResourceType.
            builder.HasOne(x => x.ResourceType)
                .WithMany()
                .HasForeignKey(x => x.ResourceTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // FK: QuestionType.
            builder.HasOne(x => x.QuestionType)
                .WithMany()
                .HasForeignKey(x => x.QuestionTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Soft delete global query filter.
            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}

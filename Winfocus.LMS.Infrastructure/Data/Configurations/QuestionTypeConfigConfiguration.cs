namespace Winfocus.LMS.Infrastructure.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Entity Framework Core configuration for <see cref="QuestionTypeConfig"/>.
    /// </summary>
    public class QuestionTypeConfigConfiguration : IEntityTypeConfiguration<QuestionTypeConfig>
    {
        /// <summary>
        /// Configures the <see cref="QuestionTypeConfig"/> entity mapping.
        /// </summary>
        /// <param name="builder">The entity type builder.</param>
        public void Configure(EntityTypeBuilder<QuestionTypeConfig> builder)
        {
            builder.ToTable("QuestionTypeConfigs");

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            // Unique: same question type name cannot repeat within same hierarchy.
            builder.HasIndex(x => new
            {
                x.SyllabusId,
                x.GradeId,
                x.SubjectId,
                x.UnitId,
                x.ChapterId,
                x.ResourceTypeId,
                x.Name,
            })
            .IsUnique()
            .HasFilter("IsDeleted = 0");

            // FK: Syllabus.
            builder.HasOne(x => x.Syllabus)
                .WithMany()
                .HasForeignKey(x => x.SyllabusId)
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

            // Soft delete global query filter.
            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}

namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Data.Configurations;

    /// <summary>
    /// ExamSubjectConfiguration.
    /// </summary>
    /// <seealso cref="Winfocus.LMS.Infrastructure.Data.Configurations.BaseEntityConfiguration&lt;Winfocus.LMS.Domain.Entities.ExamSubject&gt;" />
    public class ExamSubjectConfiguration : BaseEntityConfiguration<ExamSubject>
    {
        /// <summary>
        /// Override in derived configurations to add entity-specific
        /// columns, indexes, relationships, and constraints.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void ConfigureEntity(EntityTypeBuilder<ExamSubject> builder)
        {
            builder.ToTable("ExamSubjects");

            builder.Property(e => e.GradeId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("nvarchar(100)");

            builder.Property(e => e.Code)
                .IsRequired(false)
                .HasMaxLength(20)
                .HasColumnType("nvarchar(20)");

            builder.Property(e => e.Description)
                .IsRequired(false)
                .HasMaxLength(500)
                .HasColumnType("nvarchar(500)");

            // Unique: same subject name per grade
            builder.HasIndex(e => new { e.GradeId, e.Name })
                .IsUnique()
                .HasFilter("[IsDeleted] = 0")
                .HasDatabaseName("IX_ExamSubjects_GradeId_Name_WhereNotDeleted");
        }
    }
}

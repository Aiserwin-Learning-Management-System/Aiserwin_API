namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Data.Configurations;

    /// <summary>
    /// ExamChapterConfiguration.
    /// </summary>
    /// <seealso cref="Winfocus.LMS.Infrastructure.Data.Configurations.BaseEntityConfiguration&lt;Winfocus.LMS.Domain.Entities.ExamChapter&gt;" />
    public class ExamChapterConfiguration : BaseEntityConfiguration<ExamChapter>
    {
        /// <summary>
        /// Override in derived configurations to add entity-specific
        /// columns, indexes, relationships, and constraints.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void ConfigureEntity(EntityTypeBuilder<ExamChapter> builder)
        {
            builder.ToTable("ExamChapters");

            builder.Property(e => e.UnitId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("nvarchar(200)");

            builder.Property(e => e.ChapterNumber)
                .IsRequired()
                .HasColumnType("int");

            builder.Property(e => e.Description)
                .IsRequired(false)
                .HasMaxLength(500)
                .HasColumnType("nvarchar(500)");

            // Unique: chapter number per unit
            builder.HasIndex(e => new { e.UnitId, e.ChapterNumber })
                .IsUnique()
                .HasFilter("[IsDeleted] = 0")
                .HasDatabaseName("IX_ExamChapters_UnitId_ChapterNumber_WhereNotDeleted");

            // Unique: chapter name per unit
            builder.HasIndex(e => new { e.UnitId, e.Name })
                .IsUnique()
                .HasFilter("[IsDeleted] = 0")
                .HasDatabaseName("IX_ExamChapters_UnitId_Name_WhereNotDeleted");
        }
    }
}

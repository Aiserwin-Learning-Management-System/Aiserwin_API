namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// ExamSyllabusConfiguration.
    /// </summary>
    /// <seealso cref="Winfocus.LMS.Infrastructure.Data.Configurations.BaseEntityConfiguration&lt;Winfocus.LMS.Domain.Entities.ExamSyllabus&gt;" />
    public class ExamSyllabusConfiguration : BaseEntityConfiguration<ExamSyllabus>
    {
        /// <summary>
        /// Override in derived configurations to add entity-specific
        /// columns, indexes, relationships, and constraints.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void ConfigureEntity(EntityTypeBuilder<ExamSyllabus> builder)
        {
            builder.ToTable("ExamSyllabuses");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("nvarchar(100)");

            builder.Property(e => e.AcademicYearId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            builder.Property(e => e.Description)
                .IsRequired(false)
                .HasMaxLength(500)
                .HasColumnType("nvarchar(500)");

            // Unique: same syllabus name per academic year
            builder.HasIndex(e => new { e.Name, e.AcademicYearId })
                .IsUnique()
                .HasFilter("[IsDeleted] = 0")
                .HasDatabaseName("IX_ExamSyllabuses_Name_AcademicYearId_WhereNotDeleted");

            // FK to existing AcademicYear
            builder.HasOne(e => e.AcademicYear)
                .WithMany()
                .HasForeignKey(e => e.AcademicYearId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

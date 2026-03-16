namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Data.Configurations;

    /// <summary>
    /// ExamUnitConfiguration.
    /// </summary>
    /// <seealso cref="Winfocus.LMS.Infrastructure.Data.Configurations.BaseEntityConfiguration&lt;Winfocus.LMS.Domain.Entities.ExamUnit&gt;" />
    public class ExamUnitConfiguration : BaseEntityConfiguration<ExamUnit>
    {
        /// <summary>
        /// Override in derived configurations to add entity-specific
        /// columns, indexes, relationships, and constraints.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void ConfigureEntity(EntityTypeBuilder<ExamUnit> builder)
        {
            builder.ToTable("ExamUnits");

            builder.Property(e => e.SubjectId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("nvarchar(200)");

            builder.Property(e => e.UnitNumber)
                .IsRequired()
                .HasColumnType("int");

            builder.Property(e => e.Description)
                .IsRequired(false)
                .HasMaxLength(500)
                .HasColumnType("nvarchar(500)");

            // Unique: unit number per subject
            builder.HasIndex(e => new { e.SubjectId, e.UnitNumber })
                .IsUnique()
                .HasFilter("[IsDeleted] = 0")
                .HasDatabaseName("IX_ExamUnits_SubjectId_UnitNumber_WhereNotDeleted");

            // Unique: unit name per subject
            builder.HasIndex(e => new { e.SubjectId, e.Name })
                .IsUnique()
                .HasFilter("[IsDeleted] = 0")
                .HasDatabaseName("IX_ExamUnits_SubjectId_Name_WhereNotDeleted");
        }
    }
}

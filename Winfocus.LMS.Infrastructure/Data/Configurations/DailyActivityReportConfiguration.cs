namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Domain.Enums;
    using Winfocus.LMS.Infrastructure.Data.Configurations;

    /// <summary>
    /// DailyActivityReportConfiguration.
    /// </summary>
    /// <seealso cref="Winfocus.LMS.Infrastructure.Data.Configurations.BaseEntityConfiguration&lt;Winfocus.LMS.Domain.Entities.DailyActivityReport&gt;" />
    public class DailyActivityReportConfiguration : BaseEntityConfiguration<DailyActivityReport>
    {
        /// <summary>
        /// Override in derived configurations to add entity-specific
        /// columns, indexes, relationships, and constraints.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void ConfigureEntity(EntityTypeBuilder<DailyActivityReport> builder)
        {
            builder.ToTable("DailyActivityReports");

            builder.Property(d => d.OperatorId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            builder.Property(d => d.TaskId)
                .IsRequired(false)
                .HasColumnType("uniqueidentifier");

            builder.Property(d => d.ReportDate)
                .IsRequired()
                .HasColumnType("date");

            builder.Property(d => d.QuestionsTyped)
                .IsRequired()
                .HasDefaultValue(0)
                .HasColumnType("int");

            builder.Property(d => d.TimeSpentHours)
                .IsRequired()
                .HasDefaultValue(0m)
                .HasColumnType("decimal(4,1)");

            builder.Property(d => d.IssuesFaced)
                .IsRequired(false)
                .HasMaxLength(1000)
                .HasColumnType("nvarchar(1000)");

            builder.Property(d => d.Remarks)
                .IsRequired(false)
                .HasMaxLength(1000)
                .HasColumnType("nvarchar(1000)");

            builder.Property(d => d.Status)
            .IsRequired()
            .HasConversion<int>()
            .HasDefaultValue(DarStatus.Draft)
            .HasColumnType("int");

            // ── Indexes ──────────────────────────────────────────
            // One DAR per operator per date
            builder.HasIndex(d => new { d.OperatorId, d.ReportDate })
                .IsUnique()
                .HasFilter("[IsDeleted] = 0")
                .HasDatabaseName("IX_DailyActivityReports_OperatorId_ReportDate_WhereNotDeleted");

            builder.HasIndex(d => d.Status)
                .HasDatabaseName("IX_DailyActivityReports_Status");

            // ── Relationships ────────────────────────────────────
            builder.HasOne(d => d.Operator)
                .WithMany()
                .HasForeignKey(d => d.OperatorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

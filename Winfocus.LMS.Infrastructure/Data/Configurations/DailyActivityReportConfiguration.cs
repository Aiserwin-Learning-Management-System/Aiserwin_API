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

            // ── OperatorId ───────────────────────────────────────
            builder.Property(d => d.OperatorId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            // ── TaskId (nullable) ────────────────────────────────
            builder.Property(d => d.TaskId)
                .IsRequired(false)
                .HasColumnType("uniqueidentifier");

            // ── ReportDate ───────────────────────────────────────
            builder.Property(d => d.ReportDate)
                .IsRequired()
                .HasColumnType("date");

            // ── QuestionsTyped ───────────────────────────────────
            builder.Property(d => d.QuestionsTyped)
                .IsRequired()
                .HasDefaultValue(0)
                .HasColumnType("int");

            // ── TimeSpentHours ───────────────────────────────────
            builder.Property(d => d.TimeSpentHours)
                .IsRequired()
                .HasDefaultValue(0m)
                .HasColumnType("decimal(4,1)");

            // ── IssuesFaced ──────────────────────────────────────
            builder.Property(d => d.IssuesFaced)
                .IsRequired(false)
                .HasMaxLength(1000)
                .HasColumnType("nvarchar(1000)");

            // ── Remarks ──────────────────────────────────────────
            builder.Property(d => d.Remarks)
                .IsRequired(false)
                .HasMaxLength(1000)
                .HasColumnType("nvarchar(1000)");

            // ── Status ───────────────────────────────────────────
            builder.Property(d => d.Status)
                .IsRequired()
                .HasConversion<int>()
                .HasDefaultValue(DarStatus.Draft)
                .HasColumnType("int");

            // ── Indexes ──────────────────────────────────────────
            // One DAR per operator per date (among non-deleted)
            builder.HasIndex(d => new { d.OperatorId, d.ReportDate })
                .IsUnique()
                .HasFilter("[IsDeleted] = 0")
                .HasDatabaseName("IX_DailyActivityReports_OperatorId_ReportDate_Unique");

            builder.HasIndex(d => d.Status)
                .HasDatabaseName("IX_DailyActivityReports_Status");

            // ── Relationships ────────────────────────────────────
            builder.HasOne(d => d.Operator)
                .WithMany()
                .HasForeignKey(d => d.OperatorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.TaskAssignment)
                .WithMany(t => t.DailyReports)
                .HasForeignKey(d => d.TaskId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}

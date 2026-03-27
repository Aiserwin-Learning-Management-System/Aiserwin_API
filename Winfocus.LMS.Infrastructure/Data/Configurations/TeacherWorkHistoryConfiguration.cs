namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// EF Core configuration for <see cref="TeacherWorkHistory"/>.
    /// </summary>
    public class TeacherWorkHistoryConfiguration : BaseEntityConfiguration<TeacherWorkHistory>
    {
        /// <summary>
        /// Override in derived configurations to add entity-specific
        /// columns, indexes, relationships, and constraints.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void ConfigureEntity(EntityTypeBuilder<TeacherWorkHistory> builder)
        {
            // ── Table ────────────────────────────────────────────
            builder.ToTable("TeacherWorkHistories");

            // ── TeacherRegistrationId ───────────────────────────
            builder.Property(twh => twh.TeacherRegistrationId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            // ── Duration ─────────────────────────────────────────
            builder.Property(twh => twh.Duration)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("nvarchar(100)");

            // ── JobProfile ───────────────────────────────────────
            builder.Property(twh => twh.JobProfile)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("nvarchar(200)");

            // ── Institution ──────────────────────────────────────
            builder.Property(twh => twh.Institution)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("nvarchar(200)");

            // ── Location ─────────────────────────────────────────
            builder.Property(twh => twh.Location)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("nvarchar(200)");

            // ── ReasonForLeaving ────────────────────────────────
            builder.Property(twh => twh.ReasonForLeaving)
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnType("nvarchar(500)");

            // ── EmploymentStatus ────────────────────────────────
            builder.Property(twh => twh.EmploymentStatus)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("nvarchar(50)");

            // ── Relationships ───────────────────────────────────
            builder.HasOne(twh => twh.TeacherRegistration)
                .WithMany(tr => tr.WorkHistory)
                .HasForeignKey(twh => twh.TeacherRegistrationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// EF Core configuration for <see cref="TeacherTool"/>.
    /// </summary>
    public class TeacherToolConfiguration : BaseEntityConfiguration<TeacherTool>
    {
        /// <summary>
        /// Override in derived configurations to add entity-specific
        /// columns, indexes, relationships, and constraints.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void ConfigureEntity(EntityTypeBuilder<TeacherTool> builder)
        {
            // ── Table ────────────────────────────────────────────
            builder.ToTable("TeacherTools");

            // ── TeacherRegistrationId ───────────────────────────
            builder.Property(tt => tt.TeacherRegistrationId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            // ── ToolId ──────────────────────────────────────────
            builder.Property(tt => tt.ToolId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            // ── Indexes ─────────────────────────────────────────
            builder.HasIndex(tt => new { tt.TeacherRegistrationId, tt.ToolId })
                .IsUnique()
                .HasDatabaseName("IX_TeacherTools_TeacherRegistrationId_ToolId");

            builder.HasOne(tt => tt.Tool)
                .WithMany()
                .HasForeignKey(tt => tt.ToolId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

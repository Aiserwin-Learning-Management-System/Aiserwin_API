namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Domain.Enums;

    /// <summary>
    /// EF Core configuration for <see cref="TeacherLanguage"/>.
    /// </summary>
    public class TeacherLanguageConfiguration : BaseEntityConfiguration<TeacherLanguage>
    {
        /// <summary>
        /// Override in derived configurations to add entity-specific
        /// columns, indexes, relationships, and constraints.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void ConfigureEntity(EntityTypeBuilder<TeacherLanguage> builder)
        {
            // ── Table ────────────────────────────────────────────
            builder.ToTable("TeacherLanguages");

            // ── TeacherRegistrationId ───────────────────────────
            builder.Property(tl => tl.TeacherRegistrationId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            // ── Language (enum → int) ───────────────────────────
            builder.Property(tl => tl.Language)
                .IsRequired()
                .HasConversion<int>()
                .HasColumnType("int");

            // ── Indexes ─────────────────────────────────────────
            builder.HasIndex(tl => new { tl.TeacherRegistrationId, tl.Language })
                .IsUnique()
                .HasDatabaseName("IX_TeacherLanguages_TeacherRegistrationId_Language");
        }
    }
}

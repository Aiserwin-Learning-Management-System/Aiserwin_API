namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// StreamConfiguration.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration&lt;Winfocus.LMS.Domain.Entities.Streams&gt;" />
    public sealed class StreamConfiguration : IEntityTypeConfiguration<Streams>
    {
        /// <summary>
        /// Configures the entity of type <typeparamref name="TEntity" />.
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity type.</param>
        public void Configure(EntityTypeBuilder<Streams> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(200);

            // Composite unique — same name can't repeat under same grade
            builder.HasIndex(s => new { s.GradeId, s.Name })
                .IsUnique();

            // One grade → many streams (linked properly)
            builder.HasOne(s => s.Grade)
                .WithMany(g => g.Streams)
                .HasForeignKey(s => s.GradeId)
                .OnDelete(DeleteBehavior.Restrict);

            // One stream → many courses
            builder.HasMany(s => s.Courses)
                .WithOne(c => c.Stream)
                .HasForeignKey(c => c.StreamId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

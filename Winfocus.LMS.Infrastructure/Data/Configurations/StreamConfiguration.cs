namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// StreamCourseConfiguration.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration&lt;Winfocus.LMS.Domain.Entities.StreamCourse&gt;" />
    public sealed class StreamConfiguration : IEntityTypeConfiguration<Streams>
    {
        /// <summary>
        /// Configures the entity of type <typeparamref name="TEntity" />.
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity type.</param>
        public void Configure(EntityTypeBuilder<Streams> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.StreamName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(s => s.StreamCode)
                .HasMaxLength(50);

            // One stream → many courses
            builder.HasMany(s => s.Courses)
                .WithOne(c => c.Stream)
                .HasForeignKey(c => c.StreamId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

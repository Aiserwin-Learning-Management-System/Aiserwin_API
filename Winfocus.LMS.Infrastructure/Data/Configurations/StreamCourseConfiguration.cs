namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// StreamCourseConfiguration.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration&lt;Winfocus.LMS.Domain.Entities.StreamCourse&gt;" />
    public sealed class StreamCourseConfiguration
        : IEntityTypeConfiguration<StreamCourse>
    {
        /// <summary>
        /// Configures the entity of type <typeparamref name="TEntity" />.
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity type.</param>
        public void Configure(EntityTypeBuilder<StreamCourse> builder)
        {
            builder.HasKey(x => new { x.StreamId, x.CourseId });

            builder.HasOne(x => x.Stream)
                .WithMany(x => x.StreamCourses)
                .HasForeignKey(x => x.StreamId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Course)
                .WithMany(x => x.StreamCourses)
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

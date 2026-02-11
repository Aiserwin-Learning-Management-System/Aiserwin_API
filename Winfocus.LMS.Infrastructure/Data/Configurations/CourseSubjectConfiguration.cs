namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// CourseSubjectConfiguration.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration&lt;Winfocus.LMS.Domain.Entities.CourseSubject&gt;" />
    public sealed class CourseSubjectConfiguration
        : IEntityTypeConfiguration<CourseSubject>
    {
        /// <summary>
        /// Configures the entity of type <typeparamref name="TEntity" />.
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity type.</param>
        public void Configure(EntityTypeBuilder<CourseSubject> builder)
        {
            builder.HasKey(x => new { x.CourseId, x.SubjectId });

            builder.HasOne(x => x.Course)
                .WithMany(x => x.CourseSubjects)
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Subject)
                .WithMany(x => x.CourseSubjects)
                .HasForeignKey(x => x.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// StudentCourseDiscountConfiguration.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration&lt;Winfocus.LMS.Domain.Entities.StudentCourseDiscount&gt;" />
    public class StudentCourseDiscountConfiguration
        : IEntityTypeConfiguration<StudentCourseDiscount>
    {
        /// <summary>
        /// Configures the entity of type <typeparamref name="TEntity" />.
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity type.</param>
        public void Configure(EntityTypeBuilder<StudentCourseDiscount> builder)
        {
            builder.ToTable("StudentCourseDiscounts");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.DiscountName).HasMaxLength(200).IsRequired();
            builder.Property(x => x.DiscountPercent).HasColumnType("decimal(5,2)");

            builder.HasOne(x => x.Student)
                .WithMany()
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Course)
                .WithMany()
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.FeePlanDiscount)
                .WithMany()
                .HasForeignKey(x => x.FeePlanDiscountId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasIndex(x => new { x.StudentId, x.CourseId, x.DiscountName })
                .HasFilter("[IsDeleted] = 0");
        }
    }
}

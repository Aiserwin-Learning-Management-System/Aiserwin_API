namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// StudentFeeSelectionConfiguration.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration&lt;Winfocus.LMS.Domain.Entities.StudentFeeSelection&gt;" />
    public class StudentFeeSelectionConfiguration
        : IEntityTypeConfiguration<StudentFeeSelection>
    {
        /// <summary>
        /// Configures the entity of type <typeparamref name="TEntity" />.
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity type.</param>
        public void Configure(EntityTypeBuilder<StudentFeeSelection> builder)
        {
            builder.ToTable("StudentFeeSelections");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.YearlyFee).HasColumnType("decimal(18,2)");
            builder.Property(x => x.TotalBeforeDiscount).HasColumnType("decimal(18,2)");
            builder.Property(x => x.TotalDiscountPercent).HasColumnType("decimal(5,2)");
            builder.Property(x => x.TotalDiscountAmount).HasColumnType("decimal(18,2)");
            builder.Property(x => x.FinalAmount).HasColumnType("decimal(18,2)");

            // Store enums as strings in DB for readability
            builder.Property(x => x.PaymentType)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Status)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.HasOne(x => x.Student)
                .WithMany().HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Course)
                .WithMany().HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.FeePlan)
                .WithMany().HasForeignKey(x => x.FeePlanId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.AppliedDiscounts)
                .WithOne(x => x.StudentFeeSelection)
                .HasForeignKey(x => x.StudentFeeSelectionId);

            builder.HasMany(x => x.Installments)
                .WithOne(x => x.StudentFeeSelection)
                .HasForeignKey(x => x.StudentFeeSelectionId);

            builder.HasIndex(x => new { x.StudentId, x.CourseId })
                .HasFilter("[IsDeleted] = 0");
        }
    }
}

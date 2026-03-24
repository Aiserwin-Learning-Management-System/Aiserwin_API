namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// StudentFeeDiscountConfiguration.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration&lt;Winfocus.LMS.Domain.Entities.StudentFeeDiscount&gt;" />
    public class StudentFeeDiscountConfiguration
        : IEntityTypeConfiguration<StudentFeeDiscount>
    {
        /// <summary>
        /// Configures the entity of type <typeparamref name="TEntity" />.
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity type.</param>
        public void Configure(EntityTypeBuilder<StudentFeeDiscount> builder)
        {
            builder.ToTable("StudentFeeDiscounts");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.DiscountName).HasMaxLength(200).IsRequired();
            builder.Property(x => x.DiscountPercent).HasColumnType("decimal(5,2)");
            builder.Property(x => x.DiscountAmount).HasColumnType("decimal(18,2)");

            builder.HasOne(x => x.StudentFeeSelection)
                .WithMany(s => s.AppliedDiscounts)
                .HasForeignKey(x => x.StudentFeeSelectionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// Configures the FeePlanDiscount entity schema and common BaseEntity properties.
    /// </summary>
    public class FeePlanDiscountConfiguration : BaseEntityConfiguration<FeePlanDiscount>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<FeePlanDiscount> builder)
        {
            builder.ToTable("FeePlanDiscount");

            builder.Property(e => e.DiscountName)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(e => e.DiscountPercent)
                .HasColumnType("decimal(5,2)");
        }
    }
}

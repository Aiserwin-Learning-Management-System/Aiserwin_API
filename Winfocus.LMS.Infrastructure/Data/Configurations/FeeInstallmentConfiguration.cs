namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// Configures the FeeInstallment entity schema and common BaseEntity properties.
    /// </summary>
    public class FeeInstallmentConfiguration : BaseEntityConfiguration<FeeInstallment>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<FeeInstallment> builder)
        {
            builder.ToTable("FeeInstallments");

            builder.Property(e => e.Amount)
                .HasColumnType("decimal(18,2)");
        }
    }
}

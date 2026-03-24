namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// FeePlanConfiguration.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration&lt;Winfocus.LMS.Domain.Entities.FeePlan&gt;" />
    public class FeePlanConfiguration : IEntityTypeConfiguration<FeePlan>
    {
        /// <summary>
        /// Configures the entity of type <typeparamref name="TEntity" />.
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity type.</param>
        public void Configure(EntityTypeBuilder<FeePlan> builder)
        {
            builder.ToTable("FeePlans");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.PlanName).HasMaxLength(200).IsRequired();
            builder.Property(x => x.TuitionFee).HasColumnType("decimal(18,2)");

            // Store enum as string
            builder.Property(x => x.PaymentType)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.HasOne(x => x.Course)
                .WithMany(c => c.FeePlans)
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Subject)
                .WithMany()
                .HasForeignKey(x => x.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Discounts)
                .WithOne(x => x.FeePlan)
                .HasForeignKey(x => x.FeePlanId);

            builder.HasMany(x => x.Installments)
                .WithOne()
                .HasForeignKey(x => x.FeePlanId);
        }
    }
}

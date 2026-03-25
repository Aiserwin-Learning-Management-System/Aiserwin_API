namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// StudentInstallmentConfiguration.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration&lt;Winfocus.LMS.Domain.Entities.StudentInstallment&gt;" />
    public class StudentInstallmentConfiguration
        : IEntityTypeConfiguration<StudentInstallment>
    {
        /// <summary>
        /// Configures the entity of type <typeparamref name="TEntity" />.
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity type.</param>
        public void Configure(EntityTypeBuilder<StudentInstallment> builder)
        {
            builder.ToTable("StudentInstallments");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.DueAmount).HasColumnType("decimal(18,2)");
            builder.Property(x => x.PaidAmount).HasColumnType("decimal(18,2)");
            builder.Property(x => x.BalanceAmount).HasColumnType("decimal(18,2)");
            builder.Property(x => x.Remarks).HasMaxLength(500);

            // Store enum as string
            builder.Property(x => x.Status)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.HasOne(x => x.StudentFeeSelection)
                .WithMany(s => s.Installments)
                .HasForeignKey(x => x.StudentFeeSelectionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => new { x.StudentFeeSelectionId, x.InstallmentNo })
                .IsUnique();
        }
    }
}

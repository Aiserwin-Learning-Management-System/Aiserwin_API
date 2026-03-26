namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// QuestionOptionConfiguration.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration&lt;Winfocus.LMS.Domain.Entities.QuestionOption&gt;" />
    public class QuestionOptionConfiguration : IEntityTypeConfiguration<QuestionOption>
    {
        /// <summary>
        /// Configures the entity of type <typeparamref name="TEntity" />.
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity type.</param>
        public void Configure(EntityTypeBuilder<QuestionOption> builder)
        {
            builder.ToTable("QuestionOptions");

            builder.Property(o => o.QuestionId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            builder.Property(o => o.OptionLabel)
                .IsRequired()
                .HasMaxLength(10)
                .HasColumnType("nvarchar(10)");

            builder.Property(o => o.OptionText)
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            builder.Property(o => o.IsCorrect)
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnType("bit");
        }
    }
}

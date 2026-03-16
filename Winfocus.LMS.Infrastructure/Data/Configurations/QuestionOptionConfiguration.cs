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

            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id)
                .HasColumnType("uniqueidentifier")
                .HasDefaultValueSql("NEWSEQUENTIALID()")
                .ValueGeneratedOnAdd();

            builder.Property(o => o.QuestionId).IsRequired().HasColumnType("uniqueidentifier");

            builder.Property(o => o.OptionLabel)
                .IsRequired().HasMaxLength(10).HasColumnType("nvarchar(10)");

            builder.Property(o => o.OptionText)
                .IsRequired().HasColumnType("nvarchar(max)");

            builder.Property(o => o.DisplayOrder)
                .IsRequired().HasDefaultValue(0).HasColumnType("int");

            builder.Property(o => o.IsCorrect)
                .IsRequired().HasDefaultValue(false).HasColumnType("bit");

            builder.HasIndex(o => new { o.QuestionId, o.DisplayOrder })
                .HasDatabaseName("IX_QuestionOptions_QuestionId_DisplayOrder");

            builder.HasIndex(o => new { o.QuestionId, o.OptionLabel })
                .IsUnique()
                .HasDatabaseName("IX_QuestionOptions_QuestionId_Label_Unique");
        }
    }
}

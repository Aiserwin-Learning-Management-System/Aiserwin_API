namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Domain.Enums;

    /// <summary>
    /// QuestionReviewConfiguration.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration&lt;Winfocus.LMS.Domain.Entities.QuestionReview&gt;" />
    public class QuestionReviewConfiguration : IEntityTypeConfiguration<QuestionReview>
    {
        /// <summary>
        /// Configures the entity of type <typeparamref name="TEntity" />.
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity type.</param>
        public void Configure(EntityTypeBuilder<QuestionReview> builder)
        {
            builder.ToTable("QuestionReviews");

            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id)
                .HasColumnType("uniqueidentifier")
                .HasDefaultValueSql("NEWSEQUENTIALID()")
                .ValueGeneratedOnAdd();

            builder.Property(r => r.QuestionId).IsRequired().HasColumnType("uniqueidentifier");

            builder.Property(r => r.ReviewerId)
                .IsRequired().HasMaxLength(100).HasColumnType("nvarchar(100)");

            builder.Property(r => r.ReviewerRole)
                .IsRequired().HasMaxLength(50).HasColumnType("nvarchar(50)");

            builder.Property(r => r.Action)
                .IsRequired().HasConversion<int>().HasColumnType("int");

            builder.Property(r => r.Feedback)
                .IsRequired(false).HasMaxLength(2000).HasColumnType("nvarchar(2000)");

            builder.Property(r => r.ReviewedAt)
                .IsRequired().HasDefaultValueSql("GETUTCDATE()").HasColumnType("datetime2");

            builder.HasIndex(r => r.QuestionId)
                .HasDatabaseName("IX_QuestionReviews_QuestionId");

            builder.HasIndex(r => r.ReviewerId)
                .HasDatabaseName("IX_QuestionReviews_ReviewerId");
        }
    }
}

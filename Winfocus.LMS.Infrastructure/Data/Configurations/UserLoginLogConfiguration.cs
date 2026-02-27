namespace Winfocus.LMS.Infrastructure.Persistence.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// UserLoginLogConfiguration.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration&lt;Winfocus.LMS.Domain.Entities.UserLoginLog&gt;" />
    public class UserLoginLogConfiguration : IEntityTypeConfiguration<UserLoginLog>
    {
        /// <summary>
        /// Configures the entity of type <typeparamref name="TEntity" />.
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity type.</param>
        public void Configure(EntityTypeBuilder<UserLoginLog> builder)
        {
            builder.ToTable("UserLoginLogs");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.LoginTimestamp)
                .IsRequired()
                .HasColumnType("datetimeoffset");

            builder.Property(x => x.IpAddress)
                .HasMaxLength(45);

            builder.Property(x => x.UserAgent)
                .HasMaxLength(512);

            builder.Property(x => x.FailureReason)
                .HasMaxLength(256);

            builder.HasIndex(x => new { x.Id, x.LoginTimestamp })
                .HasDatabaseName("IX_UserLoginLogs_UserId_LoginTimestamp");

            builder.HasIndex(x => x.LoginTimestamp)
                .HasDatabaseName("IX_UserLoginLogs_LoginTimestamp");
        }
    }
}

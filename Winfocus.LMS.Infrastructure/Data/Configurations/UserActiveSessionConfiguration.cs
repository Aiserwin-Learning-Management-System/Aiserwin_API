namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// EF Core configuration for the <see cref="UserActiveSession"/> entity.
    /// </summary>
    public class UserActiveSessionConfiguration
        : IEntityTypeConfiguration<UserActiveSession>
    {
        /// <summary>
        /// Configures the entity of type <typeparamref name="TEntity" />.
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity type.</param>
        public void Configure(EntityTypeBuilder<UserActiveSession> builder)
        {
            builder.ToTable("UserActiveSessions");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.SessionId)
                .IsRequired()
                .HasMaxLength(128);

            builder.HasIndex(x => x.SessionId)
                .IsUnique()
                .HasDatabaseName("IX_UserActiveSessions_SessionId");

            builder.Property(x => x.IpAddress)
                .IsRequired()
                .HasMaxLength(45);

            builder.Property(x => x.UserAgent)
                .HasMaxLength(512);

            builder.Property(x => x.LoginAt)
                .IsRequired()
                .HasColumnType("datetimeoffset");

            builder.Property(x => x.ExpiresAt)
                .IsRequired()
                .HasColumnType("datetimeoffset");

            builder.Property(x => x.LogoutAt)
                .HasColumnType("datetimeoffset");

            builder.Property(x => x.IsRevoked)
                .HasDefaultValue(false);

            // Composite index for the most common query:
            // "Find active, non-revoked, non-expired session for a user"
            builder.HasIndex(x => new
            {
                x.UserId,
                x.IsActive,
                x.IsRevoked,
                x.ExpiresAt,
            })
            .HasDatabaseName("IX_UserActiveSessions_UserId_Active_Revoked_Expires");

            builder.HasIndex(x => x.UserId)
                .HasDatabaseName("IX_UserActiveSessions_UserId");
        }
    }
}

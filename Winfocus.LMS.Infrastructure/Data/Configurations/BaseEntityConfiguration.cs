namespace Winfocus.LMS.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// Shared EF Core configuration for all entities inheriting <see cref="BaseEntity"/>.
    /// Configures common columns: Id, audit fields, IsActive, and IsDeleted.
    /// </summary>
    /// <typeparam name="TEntity">The entity type inheriting BaseEntity.</typeparam>
    public abstract class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : BaseEntity
    {
        /// <summary>
        /// Configures the entity of type <typeparamref name="TEntity" />.
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity type.</param>
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            // ── Primary Key ──────────────────────────────────────
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasColumnType("uniqueidentifier")
                .HasDefaultValueSql("NEWSEQUENTIALID()")
                .ValueGeneratedOnAdd();

            // ── IsActive ─────────────────────────────────────────
            builder.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .HasColumnType("bit");

            // ── IsDeleted (Soft Delete) ──────────────────────────
            builder.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnType("bit");

            // ── Audit: CreatedAt ─────────────────────────────────
            builder.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()")
                .HasColumnType("datetime2");

            // ── Audit: CreatedBy ─────────────────────────────────
            builder.Property(e => e.CreatedBy)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            // ── Audit: UpdatedAt ─────────────────────────────────
            builder.Property(e => e.UpdatedAt)
                .IsRequired(false)
                .HasColumnType("datetime2");

            // ── Audit: UpdatedBy ─────────────────────────────────
            builder.Property(e => e.UpdatedBy)
                .IsRequired(false)
                .HasColumnType("uniqueidentifier");

            // ── Global Query Filter: exclude soft-deleted ────────
            builder.HasQueryFilter(e => !e.IsDeleted);

            // ── Let child classes add their own config ───────────
            ConfigureEntity(builder);
        }

        /// <summary>
        /// Override in derived configurations to add entity-specific
        /// columns, indexes, relationships, and constraints.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected abstract void ConfigureEntity(EntityTypeBuilder<TEntity> builder);
    }
}

namespace Winfocus.LMS.Domain.Common
{
    /// <summary>
    /// Represents the base entity for all domain entities, providing common properties such as Id, audit fields, and active status.
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the entity was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user who created the entity.
        /// </summary>
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the entity was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user who last updated the entity.
        /// </summary>
        public Guid? UpdatedBy { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is active.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the entity has been soft-deleted.
        /// Soft-deleted entities are excluded from normal queries via global query filters.
        /// </summary>
        public bool IsDeleted { get; set; } = false;
    }
}

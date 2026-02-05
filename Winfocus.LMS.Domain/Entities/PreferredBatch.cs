namespace Winfocus.LMS.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// Represents a preferred batch selection for an entity (for example, a student's preferred batch).
    /// </summary>
    public class PreferredBatch : BaseEntity
    {
        /// <summary>
        /// Gets or sets the display name of the preferred batch.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated batch.
        /// </summary>
        public Guid BatchId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated batch.
        /// </summary>
        public Batch Batch { get; set; } = null!;
    }
}

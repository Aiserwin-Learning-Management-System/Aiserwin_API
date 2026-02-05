namespace Winfocus.LMS.Application.DTOs.Masters
{
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Batch master data transfer object containing master-level fields.
    /// </summary>
    public class BatchDto : BaseClassDTO
    {
        /// <summary>
        /// Gets or sets the display name of the batch.
        /// </summary>
        public string BatchName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the optional code for the batch.
        /// </summary>
        public string BatchCode { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated subject.
        /// </summary>
        public Guid SubjectId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated subject.
        /// </summary>
        public SubjectDto Subject { get; set; } = null!;
    }
}

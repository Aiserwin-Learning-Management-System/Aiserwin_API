namespace Winfocus.LMS.Domain.Entities
{
    using System;
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// Represents an academic stream within a grade (for example, Science or Arts).
    /// </summary>
    public class Stream : BaseEntity
    {
        /// <summary>
        /// Gets or sets the display name of the stream.
        /// </summary>
        public string StreamName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the optional code for the stream.
        /// </summary>
        public string StreamCode { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated grade.
        /// </summary>
        public Guid GradeId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated grade.
        /// </summary>
        public Grade Grade { get; set; } = null!;
    }
}

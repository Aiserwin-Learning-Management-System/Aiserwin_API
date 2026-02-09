namespace Winfocus.LMS.Domain.Entities
{
    using System;
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// Represents a student's document records such as photo and signature.
    /// </summary>
    public class StudentDocuments : BaseEntity
    {
        /// <summary>
        /// Gets or sets the path or binary reference to the student's photo.
        /// </summary>
        public string StudentPhoto { get; set; } = null!;

        /// <summary>
        /// Gets or sets the path or binary reference to the student's signature.
        /// </summary>
        public string StudentSignature { get; set; } = null!;
    }
}

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
        public string StudentPhotoPath { get; set; } = null!;

        /// <summary>
        /// Gets or sets the path or binary reference to the student's signature.
        /// </summary>
        public string StudentSignaturePath { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether the student has accepted the Terms and Conditions.
        /// </summary>
        public bool IsAcceptedTermsAndConditions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the student has accepted the agreement.
        /// </summary>
        public bool IsAcceptedAgreement { get; set; }
    }
}

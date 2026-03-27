namespace Winfocus.LMS.Domain.Entities
{
    using Winfocus.LMS.Domain.Common;
    using Winfocus.LMS.Domain.Enums;

    /// <summary>
    /// Represents the association between a TeacherRegistration and a LanguageProficiency.
    /// </summary>
    public class TeacherLanguage : BaseEntity
    {
        /// <summary>
        /// Gets or sets the identifier of the teacher registration.
        /// </summary>
        public Guid TeacherRegistrationId { get; set; }

        /// <summary>
        /// Gets or sets the language proficiency.
        /// </summary>
        public LanguageProficiency Language { get; set; }

        /// <summary>
        /// Gets or sets the teacher registration associated with this mapping.
        /// </summary>
        public TeacherRegistration TeacherRegistration { get; set; } = null!;
    }
}
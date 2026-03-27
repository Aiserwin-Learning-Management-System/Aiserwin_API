namespace Winfocus.LMS.Domain.Entities
{
    using Winfocus.LMS.Domain.Common;
    using Winfocus.LMS.Domain.Enums;

    /// <summary>
    /// Represents the association between a TeacherRegistration and an ExamSubject.
    /// </summary>
    public class TeacherSubject : BaseEntity
    {
        /// <summary>
        /// Gets or sets the identifier of the teacher registration.
        /// </summary>
        public Guid TeacherRegistrationId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the exam subject.
        /// </summary>
        public Guid ExamSubjectId { get; set; }

        /// <summary>
        /// Gets or sets the type of subject association.
        /// </summary>
        public TeacherSubjectType Type { get; set; }

        /// <summary>
        /// Gets or sets the teacher registration associated with this mapping.
        /// </summary>
        public TeacherRegistration TeacherRegistration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the exam subject associated with this mapping.
        /// </summary>
        public ExamSubject ExamSubject { get; set; } = null!;
    }
}
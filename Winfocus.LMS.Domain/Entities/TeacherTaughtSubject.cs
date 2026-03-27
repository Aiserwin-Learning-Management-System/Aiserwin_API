namespace Winfocus.LMS.Domain.Entities
{
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// Represents the association between a TeacherRegistration and an ExamSubject for subjects taught earlier.
    /// </summary>
    public class TeacherTaughtSubject : BaseEntity
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
        /// Gets or sets the teacher registration associated with this mapping.
        /// </summary>
        public TeacherRegistration TeacherRegistration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the exam subject associated with this mapping.
        /// </summary>
        public ExamSubject ExamSubject { get; set; } = null!;
    }
}
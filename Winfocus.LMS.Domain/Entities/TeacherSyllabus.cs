namespace Winfocus.LMS.Domain.Entities
{
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// Represents the association between a TeacherRegistration and an ExamSyllabus for boards handled.
    /// </summary>
    public class TeacherSyllabus : BaseEntity
    {
        /// <summary>
        /// Gets or sets the identifier of the teacher registration.
        /// </summary>
        public Guid TeacherRegistrationId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the exam syllabus.
        /// </summary>
        public Guid ExamSyllabusId { get; set; }

        /// <summary>
        /// Gets or sets the teacher registration associated with this mapping.
        /// </summary>
        public TeacherRegistration TeacherRegistration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the exam syllabus associated with this mapping.
        /// </summary>
        public ExamSyllabus ExamSyllabus { get; set; } = null!;
    }
}
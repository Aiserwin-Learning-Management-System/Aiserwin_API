namespace Winfocus.LMS.Domain.Entities
{
    using Winfocus.LMS.Domain.Common;
    using Winfocus.LMS.Domain.Enums;

    /// <summary>
    /// Represents the association between a TeacherRegistration and an ExamGrade.
    /// </summary>
    public class TeacherGrade : BaseEntity
    {
        /// <summary>
        /// Gets or sets the identifier of the teacher registration.
        /// </summary>
        public Guid TeacherRegistrationId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the exam grade.
        /// </summary>
        public Guid ExamGradeId { get; set; }

        /// <summary>
        /// Gets or sets the type of grade association.
        /// </summary>
        public TeacherGradeType Type { get; set; }

        /// <summary>
        /// Gets or sets the teacher registration associated with this mapping.
        /// </summary>
        public TeacherRegistration TeacherRegistration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the exam grade associated with this mapping.
        /// </summary>
        public ExamGrade ExamGrade { get; set; } = null!;
    }
}
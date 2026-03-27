namespace Winfocus.LMS.Domain.Entities
{
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// Represents an academic record for a teacher registration.
    /// </summary>
    public class TeacherAcademicRecord : BaseEntity
    {
        /// <summary>
        /// Gets or sets the identifier of the teacher registration.
        /// </summary>
        public Guid TeacherRegistrationId { get; set; }

        /// <summary>
        /// Gets or sets the course name.
        /// </summary>
        public string CourseName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the marks percentage.
        /// </summary>
        public decimal MarksPercentage { get; set; }

        /// <summary>
        /// Gets or sets the subjects.
        /// </summary>
        public string Subjects { get; set; } = null!;

        /// <summary>
        /// Gets or sets the teacher registration associated with this record.
        /// </summary>
        public TeacherRegistration TeacherRegistration { get; set; } = null!;
    }
}
namespace Winfocus.LMS.Application.DTOs.Students
{

    using Winfocus.LMS.Application.DTOs.Masters;

    /// <summary>
    /// Represents a student and references to their related details entities.
    /// </summary>
    public class StudentDto : BaseClassDTO
    {
        /// <summary>
        /// Gets or sets the identifier of the student's academic details.
        /// </summary>
        public Guid StudentAcademicId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the student's academic details.
        /// </summary>
        public StudentAcademicdetailsDto AcademicDetails { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the student's personal details.
        /// </summary>
        public Guid StudentPersonalId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the student's personal details.
        /// </summary>
        public StudentPersonaldetailsdto PersonalDetails { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the student's documents record.
        /// </summary>
        public Guid StudentDocumentsId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the student's documents.
        /// </summary>
        public StudentDocumentsDto StudentDocuments { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the userid.
        /// </summary>
        public Guid Userid { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the status.
        /// </summary>
        public string Status { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the RegistraionNumber.
        /// </summary>
        public string RegistraionNumber { get; set; } = null!;

        /// <summary>
        /// Gets or sets the courses.
        /// </summary>
        /// <value>
        /// The courses.
        /// </value>
        public List<CourseDto> Courses { get; set; } = new ();
    }
}

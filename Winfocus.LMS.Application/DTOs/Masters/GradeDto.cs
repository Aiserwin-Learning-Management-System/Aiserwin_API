namespace Winfocus.LMS.Application.DTOs.Masters
{
    /// <summary>
    /// Represents an academic grade (class/level) within a syllabus.
    /// </summary>
    public class GradeDto : BaseClassDTO
    {
        /// <summary>
        /// Gets or sets the display name of the grade.
        /// </summary>
        public string GradeName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the optional code for the grade.
        /// </summary>
        public string GradeCode { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated syllabus.
        /// </summary>
        public Guid SyllabusId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated syllabus.
        /// </summary>
        public SyllabusDto Syllabus { get; set; } = null!;
    }
}

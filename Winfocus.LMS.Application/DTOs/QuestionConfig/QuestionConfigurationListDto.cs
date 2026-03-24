namespace Winfocus.LMS.Application.DTOs.QuestionConfig
{
    using System;

    /// <summary>
    /// Lightweight DTO for listing Question Configurations in a grid or table view.
    /// </summary>
    public class QuestionConfigurationListDto
    {
        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the Question Code.
        /// </summary>
        public string QuestionCode { get; set; } = default!;

        /// <summary>
        /// Gets or sets the syllabus name.
        /// </summary>
        public string SyllabusName { get; set; } = default!;

        /// <summary>
        /// Gets or sets the academic year.
        /// </summary>
        public string AcademicYear { get; set; } = default!;

        /// <summary>
        /// Gets or sets the grade name.
        /// </summary>
        public string GradeName { get; set; } = default!;

        /// <summary>
        /// Gets or sets the subject code.
        /// </summary>
        public string SubjectCode { get; set; } = default!;

        /// <summary>
        /// Gets or sets the chapter name.
        /// </summary>
        public string ChapterName { get; set; } = default!;

        /// <summary>
        /// Gets or sets the question type code.
        /// </summary>
        public string QuestionTypeCode { get; set; } = default!;

        /// <summary>
        /// Gets or sets the sequence number.
        /// </summary>
        public int SequenceNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the configuration is active.
        /// </summary>
        public bool IsActive { get; set; }
    }
}

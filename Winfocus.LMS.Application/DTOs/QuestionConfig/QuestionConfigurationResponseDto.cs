namespace Winfocus.LMS.Application.DTOs.QuestionConfig
{
    using System;

    /// <summary>
    /// Response DTO containing full Question Configuration details with resolved master names.
    /// </summary>
    public class QuestionConfigurationResponseDto
    {
        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the unique Question Code.
        /// Example: CBSE-2025-12-PHY-U01-CH01-MCQ-0001.
        /// </summary>
        public string QuestionCode { get; set; } = default!;

        /// <summary>
        /// Gets or sets the sequence number within the scope.
        /// </summary>
        public int SequenceNumber { get; set; }

        /// <summary>
        /// Gets or sets the syllabus name.
        /// </summary>
        public string SyllabusName { get; set; } = default!;

        /// <summary>
        /// Gets or sets the academic year name.
        /// Example: 2025-2026.
        /// </summary>
        public string AcademicYear { get; set; } = default!;

        /// <summary>
        /// Gets or sets the grade name.
        /// </summary>
        public string GradeName { get; set; } = default!;

        /// <summary>
        /// Gets or sets the subject name.
        /// </summary>
        public string SubjectName { get; set; } = default!;

        /// <summary>
        /// Gets or sets the subject code.
        /// Example: PHY.
        /// </summary>
        public string? SubjectCode { get; set; }

        /// <summary>
        /// Gets or sets the unit name.
        /// </summary>
        public string UnitName { get; set; } = default!;

        /// <summary>
        /// Gets or sets the unit number.
        /// </summary>
        public int UnitNumber { get; set; }

        /// <summary>
        /// Gets or sets the chapter name.
        /// </summary>
        public string ChapterName { get; set; } = default!;

        /// <summary>
        /// Gets or sets the chapter number.
        /// </summary>
        public int ChapterNumber { get; set; }

        /// <summary>
        /// Gets or sets the resource type name.
        /// </summary>
        public string ResourceTypeName { get; set; } = default!;

        /// <summary>
        /// Gets or sets the question type name.
        /// </summary>
        public string QuestionTypeName { get; set; } = default!;

        /// <summary>
        /// Gets or sets the question type code.
        /// Example: MCQ.
        /// </summary>
        public string QuestionTypeCode { get; set; } = default!;

        /// <summary>
        /// Gets or sets the creation timestamp.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}

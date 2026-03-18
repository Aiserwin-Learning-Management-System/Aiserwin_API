using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Application.DTOs.Masters
{
    /// <summary>
    /// ExamGradeDto.
    /// </summary>
    public class ExamGradeDto : BaseClassDTO
    {
        /// <summary>
        /// Gets or sets the syllabus identifier.
        /// </summary>
        public Guid SyllabusId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; } = default!;

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the syllabus.
        /// </summary>
        public ExamSyllabusDto? Syllabus { get; set; }
    }
}

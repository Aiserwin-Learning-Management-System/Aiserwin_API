using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Application.DTOs.Masters
{
    /// <summary>
    /// ExamSubjectDto.
    /// </summary>
    public class ExamSubjectDto : BaseClassDTO
    {
        /// <summary>
        /// Gets or sets the grade identifier.
        /// </summary>
        /// <value>
        /// The grade identifier.
        /// </value>
        public Guid GradeId { get; set; }

        /// <summary>
        ///  Gets or sets the grade Name.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        ///  Gets or sets the grade Code.
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// Gets or sets the grade Description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the grade Grade.
        /// </summary>
        public ExamGradeDto? Grade { get; set; }
    }
}

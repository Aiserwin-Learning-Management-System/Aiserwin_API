using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Application.DTOs.Masters
{
    /// <summary>
    /// ExamSyllabusDto.
    /// </summary>
    public class ExamSyllabusDto : BaseClassDTO
    {
        /// <summary>
        /// Gets or sets get or set Name.
        /// </summary>
        public string Name { get; set; } = default!;

        /// <summary>
        /// Gets or sets get or set AcademicYearId.
        /// </summary>
        public Guid AcademicYearId { get; set; }

        /// <summary>
        /// Gets or sets get or set Description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets get or set AcademicYear.
        /// </summary>
        public AcademicYearDto AcademicYear { get; set; } = null!;
    }
}

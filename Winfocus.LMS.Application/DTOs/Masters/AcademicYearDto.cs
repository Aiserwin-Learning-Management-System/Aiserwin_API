using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Application.DTOs.Masters
{
    /// <summary>
    /// academic year master data transfer object containing master-level fields.
    /// </summary>
    public class AcademicYearDto : BaseClassDTO
    {
        /// <summary>
        /// Gets or sets the display name of the academic year.
        /// </summary>
        public string Name { get; set; } = null!;
    }
}

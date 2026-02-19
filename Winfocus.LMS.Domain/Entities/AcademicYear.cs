using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Domain.Common;

namespace Winfocus.LMS.Domain.Entities
{
    /// <summary>
    /// Represents an academic year in the learning management system.
    /// Example: 2025–2026 academic session.
    /// </summary>
    public class AcademicYear : BaseEntity
    {
        /// <summary>
        /// Gets or sets the display name of the academic year.
        /// Example: "2025-2026".
        /// </summary>
        public string Name { get; set; } = default!;
    }
}

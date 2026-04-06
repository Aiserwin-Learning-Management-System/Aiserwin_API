namespace Winfocus.LMS.Domain.Entities
{
    using System;
    using Winfocus.LMS.Domain.Common;

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

        /// <summary>
        /// Gets or sets the starting date of the academic year.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the ending date of the academic year.
        /// </summary>
        public DateTime EndDate { get; set; }
    }
}

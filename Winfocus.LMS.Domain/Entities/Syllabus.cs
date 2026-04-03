namespace Winfocus.LMS.Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// Represents a syllabus offered by a centre.
    /// </summary>
    public class Syllabus : BaseEntity
    {
        /// <summary>
        /// Gets or sets the display name of the syllabus.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated Center.
        /// </summary>
        public Guid CenterId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated Center.
        /// </summary>
        public Center Center { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated AcademicYear.
        /// </summary>
        public Guid AcademicYearId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated AcademicYear.
        /// </summary>
        public AcademicYear AcademicYear { get; set; } = null!;
    }
}

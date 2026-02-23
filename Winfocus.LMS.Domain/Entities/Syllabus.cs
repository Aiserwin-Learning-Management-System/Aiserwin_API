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
    }
}

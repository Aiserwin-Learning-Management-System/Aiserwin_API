using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Domain.Entities
{
    /// <summary>
    /// Represents the association between a Stream and a course.
    /// </summary>
    public class StreamCourse
    {
        /// <summary>
        /// Gets or sets the identifier of the Stream.
        /// </summary>
        public Guid StreamId { get; set; }

        /// <summary>
        /// Gets or sets the Stream associated with this mapping.
        /// </summary>
        public Streams Stream { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the Course.
        /// </summary>
        public Guid CourseId { get; set; }

        /// <summary>
        /// Gets or sets the Course associated with this mapping.
        /// </summary>
        public Course Course { get; set; } = null!;
    }
}

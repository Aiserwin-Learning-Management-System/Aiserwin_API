using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Domain.Entities
{
    /// <summary>
    /// Represents uploaded document paths for a teacher.
    /// </summary>
    public class TeacherDocumentInfo
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the TeacherId.
        /// </summary>
        public Guid TeacherId { get; set; }

        /// <summary>
        /// Gets or sets the profile photo path.
        /// </summary>
        public string? PhotoPath { get; set; }

        /// <summary>
        /// Gets or sets the ID proof document path.
        /// </summary>
        public string? IdCardPath { get; set; }
    }
}

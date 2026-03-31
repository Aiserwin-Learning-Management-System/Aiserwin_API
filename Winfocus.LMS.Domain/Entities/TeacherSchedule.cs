using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Domain.Entities
{
    /// <summary>
    /// Represents the teaching schedule of a teacher.
    /// </summary>
    public class TeacherSchedule
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the teacher identifier.
        /// </summary>
        public Guid TeacherId { get; set; }

        /// <summary>
        /// Gets or sets the weekly availability slots.
        /// </summary>
        public ICollection<TeacherAvailability> Availabilities { get; set; } = new List<TeacherAvailability>();
    }
}

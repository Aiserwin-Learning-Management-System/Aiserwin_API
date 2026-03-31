using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Domain.Entities
{
    /// <summary>
    /// Represents a single availability time slot for a teacher.
    /// </summary>
    public class TeacherAvailability
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the ScheduleId.
        /// </summary>
        public Guid ScheduleId { get; set; }

        /// <summary>
        /// Gets or sets the day of the week.
        /// </summary>
        public DayOfWeek Day { get; set; }

        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time.
        /// </summary>
        public TimeSpan EndTime { get; set; }
    }
}

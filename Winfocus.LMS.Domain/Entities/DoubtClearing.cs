namespace Winfocus.LMS.Domain.Entities
{
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// Represents a Doubt Clearing .
    /// </summary>
    public class DoubtClearing : BaseEntity
    {
        /// <summary>
        /// Gets or sets the textual representation of the schedule time (for example, "09:00-10:00").
        /// </summary>
        public DateTime ScheduleTime { get; set; }

        /// <summary>
        /// Gets or sets the textual representation of the schedule end time (for example, "09:00-10:00").
        /// </summary>
        public DateTime ScheduleEndTime { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated subject.
        /// </summary>
        public Guid SubjectId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated subject.
        /// </summary>
        public Subject Subject { get; set; } = null!;
    }
}

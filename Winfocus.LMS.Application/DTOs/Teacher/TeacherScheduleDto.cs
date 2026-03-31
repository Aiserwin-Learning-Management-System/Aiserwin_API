namespace Winfocus.LMS.Application.DTOs.Teacher
{
    using System;

    /// <summary>
    /// DTO for teacher schedule information.
    /// </summary>
    public class TeacherScheduleDto
    {
        /// <summary>
        /// Gets or sets the availability entries for the week.
        /// </summary>
        public List<TeacherAvailabilityDto> Availabilities { get; set; } = new List<TeacherAvailabilityDto>();
    }

    /// <summary>
    /// DTO for a single availability entry.
    /// </summary>
    public class TeacherAvailabilityDto
    {
        /// <summary>
        /// Gets or sets the day of week (0-6).
        /// </summary>
        public int Day { get; set; }

        /// <summary>
        /// Gets or sets the start time (HH:mm).
        /// </summary>
        public string StartTime { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the end time (HH:mm).
        /// </summary>
        public string EndTime { get; set; } = string.Empty;
    }
}

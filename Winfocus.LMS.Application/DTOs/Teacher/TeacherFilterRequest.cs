namespace Winfocus.LMS.Application.DTOs.Teacher
{
    using System;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Domain.Enums;

    /// <summary>
    /// Request DTO for filtering teacher registrations.
    /// </summary>
    public class TeacherFilterRequest : PagedRequest
    {
        /// <summary>
        /// Gets or sets the country identifier.
        /// </summary>
        public Guid? CountryId { get; set; }

        /// <summary>
        /// Gets or sets the state identifier.
        /// </summary>
        public Guid? StateId { get; set; }

        /// <summary>
        /// Gets or sets the start date for creation filter.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date for creation filter.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the teacher status filter.
        /// </summary>
        public TeacherStatus? Status { get; set; }

        /// <summary>
        /// Gets or sets search text for name/email/employee id.
        /// </summary>
        public string? SearchText { get; set; }
    }
}

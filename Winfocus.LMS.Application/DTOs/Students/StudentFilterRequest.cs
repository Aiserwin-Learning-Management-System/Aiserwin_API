namespace Winfocus.LMS.Application.DTOs.Students
{
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Domain.Enums;

    /// <summary>
    /// StudentFilterRequest.
    /// </summary>
    /// <seealso cref="Winfocus.LMS.Application.DTOs.Common.PagedRequest" />
    public class StudentFilterRequest : PagedRequest
    {
        /// <summary>
        /// Gets or sets the country identifier.
        /// </summary>
        /// <value>
        /// The country identifier.
        /// </value>
        public Guid? CountryId { get; set; }

        /// <summary>
        /// Gets or sets the state identifier.
        /// </summary>
        /// <value>
        /// The state identifier.
        /// </value>
        public Guid? StateId { get; set; }

        /// <summary>
        /// Gets or sets the mode identifier.
        /// </summary>
        /// <value>
        /// The mode identifier.
        /// </value>
        public Guid? ModeId { get; set; }

        /// <summary>
        /// Gets or sets the centre identifier.
        /// </summary>
        /// <value>
        /// The centre identifier.
        /// </value>
        public Guid? CentreId { get; set; }

        /// <summary>
        /// Gets or sets the batch identifier.
        /// </summary>
        /// <value>
        /// The batch identifier.
        /// </value>
        public Guid? BatchId { get; set; }

        /// <summary>
        /// Gets or sets the grade identifier.
        /// </summary>
        /// <value>
        /// The grade identifier.
        /// </value>
        public Guid? GradeId { get; set; }

        /// <summary>
        /// Gets or sets the course identifier.
        /// </summary>
        /// <value>
        /// The course identifier.
        /// </value>
        public Guid? CourseId { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the registration status.
        /// </summary>
        /// <value>
        /// The registration status.
        /// </value>
        public RegistrationStatus? RegistrationStatus { get; set; }

        /// <summary>
        /// Gets or sets the search text.
        /// </summary>
        /// <value>
        /// The search text.
        /// </value>
        public string? SearchText { get; set; }
    }
}

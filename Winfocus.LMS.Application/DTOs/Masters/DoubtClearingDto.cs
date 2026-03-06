namespace Winfocus.LMS.Application.DTOs.Masters
{
    /// <summary>
    /// doubt clearing master data transfer object containing master-level fields.
    /// </summary>
    public class DoubtClearingDto : BaseClassDTO
    {
        /// <summary>
        /// Gets or sets the starting date of the academic year.
        /// </summary>
        public string ScheduleStartDate { get; set; }

        /// <summary>
        /// Gets or sets the ending date of the academic year.
        /// </summary>
        public string ScheduleEndDate { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated Subject.
        /// </summary>
        public Guid SubjectId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated Subject.
        /// </summary>
        public SubjectDto Subject { get; set; } = null!;
    }
}

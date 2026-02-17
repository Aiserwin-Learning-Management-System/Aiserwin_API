namespace Winfocus.LMS.Domain.Entities
{
    /// <summary>
    /// Represents the association between a Student and a BatchTimingSunday.
    /// </summary>
    public class StudentBatchTimingSunday
    {
        /// <summary>
        /// Gets or sets the identifier of the student.
        /// </summary>
        public Guid StudentId { get; set; }

        /// <summary>
        /// Gets or sets the student associated with this mapping.
        /// </summary>
        public Student Student { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the BatchTimingSunday.
        /// </summary>
        public Guid BatchTimingSundayId { get; set; }

        /// <summary>
        /// Gets or sets the BatchTimingSunday associated with this mapping.
        /// </summary>
        public BatchTimingSunday BatchTimingSunday { get; set; } = null!;
    }
}

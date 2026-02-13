namespace Winfocus.LMS.Domain.Entities
{
    /// <summary>
    /// Represents the association between a Student and a BatchTimingSaturday.
    /// </summary>
    public class StudentBatchTimingSaturday
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
        /// Gets or sets the identifier of the BatchTimingSaturday.
        /// </summary>
        public Guid BatchTimingSaturdayId { get; set; }

        /// <summary>
        /// Gets or sets the BatchTimingSaturday associated with this mapping.
        /// </summary>
        public BatchTimingSaturday BatchTimingSaturday { get; set; } = null!;
    }
}

namespace Winfocus.LMS.Domain.Entities
{
    /// <summary>
    /// Represents the association between a Student and a BatchTimingMTF.
    /// </summary>
    public class StudentBatchTimingMTF
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
        /// Gets or sets the identifier of the BatchTimingMTF.
        /// </summary>
        public Guid BatchTimingMTFId { get; set; }

        /// <summary>
        /// Gets or sets the BatchTimingMTF associated with this mapping.
        /// </summary>
        public BatchTimingMTF BatchTimingMTF { get; set; } = null!;
    }
}

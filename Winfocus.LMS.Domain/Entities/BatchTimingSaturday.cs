namespace Winfocus.LMS.Domain.Entities
{
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// Represents a batch timing entry for Saturday schedule.
    /// </summary>
    public class BatchTimingSaturday : BaseEntity
    {
        /// <summary>
        /// Gets or sets the textual representation of the batch time (for example, "09:00-10:00").
        /// </summary>
        public string BatchTime { get; set; } = null!;
    }
}

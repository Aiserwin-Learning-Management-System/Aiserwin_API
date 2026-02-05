namespace Winfocus.LMS.Domain.Entities
{
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// Represents a batch timing entry for Monday, Tuesday, Friday (or other schedule mapping).
    /// </summary>
    public class BatchTimingMTF : BaseEntity
    {
        /// <summary>
        /// Gets or sets the textual representation of the batch time (for example, "09:00-10:00").
        /// </summary>
        public string BatchTime { get; set; } = null!;
    }
}

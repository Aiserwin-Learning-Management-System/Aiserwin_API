namespace Winfocus.LMS.Domain.Enums
{
    /// <summary>
    /// Defines the review lifecycle status of a question.
    /// </summary>
    public enum QuestionStatus
    {
        /// <summary>
        /// The draft
        /// </summary>
        Draft = 0,

        /// <summary>
        /// The submitted
        /// </summary>
        Submitted = 1,

        /// <summary>
        /// The under review
        /// </summary>
        UnderReview = 2,

        /// <summary>
        /// The approved
        /// </summary>
        Approved = 3,

        /// <summary>
        /// The rejected
        /// </summary>
        Rejected = 4,
    }
}

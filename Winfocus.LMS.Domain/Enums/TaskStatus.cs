namespace Winfocus.LMS.Domain.Enums
{
    /// <summary>
    /// Defines the lifecycle status of a task assignment.
    /// </summary>
    public enum TaskStatus
    {
        /// <summary>
        /// The pending
        /// </summary>
        Pending = 0,

        /// <summary>
        /// The in progress
        /// </summary>
        InProgress = 1,

        /// <summary>
        /// The completed
        /// </summary>
        Completed = 2,

        /// <summary>
        /// The overdue
        /// </summary>
        Overdue = 3,
    }
}

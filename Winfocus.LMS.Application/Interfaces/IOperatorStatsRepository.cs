namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// Repository interface for operator productivity statistics data access.
    /// All database queries for stats aggregation go through this repository.
    /// </summary>
    public interface IOperatorStatsRepository
    {
        /// <summary>
        /// Gets the operator name from staff registration values.
        /// Searches for field names containing "name" or "full_name".
        /// </summary>
        /// <param name="operatorId">The operator registration identifier.</param>
        /// <returns>The operator name if found; otherwise "Unknown".</returns>
        Task<string> GetOperatorNameAsync(Guid operatorId);

        /// <summary>
        /// Gets all distinct operator IDs that have task assignments.
        /// </summary>
        /// <returns>List of operator registration identifiers.</returns>
        Task<List<Guid>> GetAllOperatorIdsWithTasksAsync();

        /// <summary>
        /// Gets the total number of tasks for an operator.
        /// </summary>
        /// <param name="operatorId">The operator registration identifier.</param>
        /// <returns>Total task count.</returns>
        Task<int> GetTotalTaskCountAsync(Guid operatorId);

        /// <summary>
        /// Gets the count of active (in-progress) tasks for an operator.
        /// </summary>
        /// <param name="operatorId">The operator registration identifier.</param>
        /// <returns>Active task count.</returns>
        Task<int> GetActiveTaskCountAsync(Guid operatorId);

        /// <summary>
        /// Gets the count of completed tasks for an operator.
        /// </summary>
        /// <param name="operatorId">The operator registration identifier.</param>
        /// <returns>Completed task count.</returns>
        Task<int> GetCompletedTaskCountAsync(Guid operatorId);

        /// <summary>
        /// Gets the count of overdue tasks for an operator.
        /// Tasks are overdue if deadline has passed and status is not Completed.
        /// </summary>
        /// <param name="operatorId">The operator registration identifier.</param>
        /// <returns>Overdue task count.</returns>
        Task<int> GetOverdueTaskCountAsync(Guid operatorId);

        /// <summary>
        /// Gets the total number of questions assigned to an operator across all tasks.
        /// </summary>
        /// <param name="operatorId">The operator registration identifier.</param>
        /// <returns>Total questions assigned.</returns>
        Task<int> GetTotalQuestionsAssignedAsync(Guid operatorId);

        /// <summary>
        /// Gets question counts grouped by status for an operator within a date range.
        /// </summary>
        /// <param name="operatorId">The operator registration identifier.</param>
        /// <param name="startDate">The period start date.</param>
        /// <param name="endDate">The period end date.</param>
        /// <returns>Dictionary of status (int) to count.</returns>
        Task<Dictionary<int, int>> GetQuestionCountsByStatusAsync(
            Guid operatorId,
            DateTime startDate,
            DateTime endDate);

        /// <summary>
        /// Gets the total questions typed and hours spent from submitted daily activity reports.
        /// </summary>
        /// <param name="operatorId">The operator registration identifier.</param>
        /// <param name="startDate">The period start date.</param>
        /// <param name="endDate">The period end date.</param>
        /// <returns>A tuple of (totalQuestionsTyped, totalHoursSpent, reportCount).</returns>
        Task<(int TotalQuestionsTyped, decimal TotalHoursSpent, int ReportCount)> GetDarAggregatesAsync(
            Guid operatorId,
            DateOnly startDate,
            DateOnly endDate);

        /// <summary>
        /// Gets daily trend data showing questions created per day within a date range.
        /// </summary>
        /// <param name="operatorId">The operator registration identifier.</param>
        /// <param name="startDate">The period start date.</param>
        /// <param name="endDate">The period end date.</param>
        /// <returns>List of date and question count pairs ordered by date.</returns>
        Task<List<(DateTime Date, int Count)>> GetDailyTrendAsync(
            Guid operatorId,
            DateTime startDate,
            DateTime endDate);

        /// <summary>
        /// Gets the staff registration identifier (operatorId) from a user account identifier.
        /// Used to convert JWT UserId to the OperatorId used in task assignments and questions.
        /// </summary>
        /// <param name="userId">The user account identifier from JWT.</param>
        /// <returns>The staff registration identifier if found; otherwise null.</returns>
        Task<Guid?> GetOperatorIdByUserIdAsync(Guid userId);
    }
}

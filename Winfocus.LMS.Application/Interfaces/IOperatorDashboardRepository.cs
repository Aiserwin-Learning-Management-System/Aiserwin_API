namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// IOperatorDashboardRepository.
    /// </summary>
    public interface IOperatorDashboardRepository
    {
        /// <summary>
        /// Finds operator's registration by the UserId (CreatedBy in StaffRegistration).
        /// Also checks User.StaffCategoryId to find matching registration.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<StaffRegistration?> GetOperatorRegistrationAsync(Guid userId);

        /// <summary>
        /// Gets active tasks (Pending/InProgress) for an operator.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        Task<List<TaskAssignment>> GetActiveTasksAsync(Guid operatorId);

        /// <summary>
        /// Gets queryable tasks for pagination.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        IQueryable<TaskAssignment> QueryTasks(Guid operatorId);

        /// <summary>
        /// Gets question counts grouped by status.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns></returns>
        Task<Dictionary<int, int>> GetQuestionCountsByStatusAsync(
            Guid operatorId, DateTime? fromDate, DateTime? toDate);

        /// <summary>
        /// Gets total questions target from tasks.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns></returns>
        Task<int> GetTotalQuestionsTargetAsync(
            Guid operatorId, DateTime? fromDate, DateTime? toDate);

        /// <summary>
        /// Gets task count for an operator within date range.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns></returns>
        Task<int> GetTaskCountAsync(
            Guid operatorId, DateTime? fromDate, DateTime? toDate);

        /// <summary>
        /// Gets rejected questions with review feedback.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="take">The take.</param>
        /// <returns></returns>
        Task<List<Question>> GetRejectedQuestionsAsync(Guid operatorId, int take = 5);

        /// <summary>
        /// Gets registration sequence number for EmployeeId.
        /// </summary>
        /// <param name="registrationId">The registration identifier.</param>
        /// <returns></returns>
        Task<int> GetRegistrationSequenceAsync(Guid registrationId);

        /// <summary>
        /// Gets the user's staff category name.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<string?> GetUserStaffCategoryAsync(Guid userId);
    }
}

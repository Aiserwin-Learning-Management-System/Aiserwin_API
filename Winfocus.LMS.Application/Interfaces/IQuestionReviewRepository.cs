namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// IQuestionReviewRepository.
    /// </summary>
    public interface IQuestionReviewRepository
    {
        /// <summary>
        /// Gets the question with details asynchronous.
        /// </summary>
        /// <param name="questionId">The question identifier.</param>
        /// <returns>Task.<Question?>.</returns>
        Task<Question?> GetQuestionWithDetailsAsync(Guid questionId);

        /// <summary>
        /// Queries the submitted questions.
        /// </summary>
        /// <returns>IQueryable.<Question>.</returns>
        IQueryable<Question> QuerySubmittedQuestions();

        /// <summary>
        /// Queries the rejected by operator.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        IQueryable<Question> QueryRejectedByOperator(Guid operatorId);

        /// <summary>
        /// Counts the by status asynchronous.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        Task<int> CountByStatusAsync(int status);

        /// <summary>
        /// Counts the reviewed today asynchronous.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        Task<int> CountReviewedTodayAsync(int action);

        /// <summary>
        /// Counts all by action asynchronous.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        Task<int> CountAllByActionAsync(int action);

        /// <summary>
        /// Adds the review asynchronous.
        /// </summary>
        /// <param name="review">The review.</param>
        /// <returns></returns>
        Task AddReviewAsync(QuestionReview review);

        /// <summary>
        /// Updates the question asynchronous.
        /// </summary>
        /// <param name="question">The question.</param>
        /// <returns></returns>
        Task UpdateQuestionAsync(Question question);

        /// <summary>
        /// Updates the task assignment asynchronous.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <returns></returns>
        Task UpdateTaskAssignmentAsync(TaskAssignment task);

        /// <summary>
        /// Saves the changes asynchronous.
        /// </summary>
        /// <returns></returns>
        Task SaveChangesAsync();
    }
}

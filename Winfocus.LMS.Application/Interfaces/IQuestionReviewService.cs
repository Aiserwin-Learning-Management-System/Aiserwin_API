namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.Review;

    /// <summary>
    /// Admin/Academic Head — review submitted questions.
    /// </summary>
    public interface IQuestionReviewService
    {
        /// <summary>
        /// Lists submitted questions pending review with hierarchy filters.
        /// </summary>
        /// <param name="request">Includes pagination and optional filters (year, syllabus, grade, subject).</param>
        /// <returns>Paged list of questions with basic info for review.</returns>
        Task<CommonResponse<PagedResult<ReviewQuestionListDto>>> GetPendingReviewsAsync(
            ReviewFilterRequest request);

        /// <summary>
        /// Gets full question detail for review including options and history.
        /// </summary>
        /// <param name="questionId">ID of the question to review.</param>
        /// <returns>Detailed question info for review.</returns>
        Task<CommonResponse<ReviewQuestionDetailDto>> GetQuestionForReviewAsync(
            Guid questionId);

        /// <summary>
        /// Approves a submitted question.
        /// </summary>
        /// <param name="questionId">ID of the question to approve.</param>
        /// <param name="reviewerId">The reviewer identifier.</param>
        /// <param name="reviewerRole">The reviewer role.</param>
        /// <param name="dto">The dto.</param>
        /// <returns>True if approval is successful; otherwise, false.</returns>
        Task<CommonResponse<bool>> ApproveAsync(
            Guid questionId, Guid reviewerId, string reviewerRole, ApproveQuestionDto? dto);

        /// <summary>
        /// Rejects a submitted question with mandatory feedback.
        /// Decrements TaskAssignment.CompletedCount.
        /// </summary>
        /// <param name="questionId">The question identifier.</param>
        /// <param name="reviewerId">The reviewer identifier.</param>
        /// <param name="reviewerRole">The reviewer role.</param>
        /// <param name="dto">The dto.</param>
        /// <returns>True if rejection is successful; otherwise, false.</returns>
        Task<CommonResponse<bool>> RejectAsync(
            Guid questionId, Guid reviewerId, string reviewerRole, RejectQuestionDto dto);

        /// <summary>
        /// Gets review dashboard stats (pending, approved today, rejected today).
        /// </summary>
        /// <returns>Aggregated review stats for dashboard display.</returns>
        Task<CommonResponse<ReviewStatsDto>> GetReviewStatsAsync();
    }
}

namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.Review;

    /// <summary>
    /// Operator — view rejected questions and fix + resubmit.
    /// </summary>
    public interface IQuestionCorrectionService
    {
        /// <summary>
        /// Gets operator's rejected questions with feedback and hierarchy.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>Paged list of rejected questions with feedback for correction.</returns>
        Task<CommonResponse<PagedResult<CorrectionListDto>>> GetMyCorrectionsAsync(
            Guid userId, PagedRequest request);

        /// <summary>
        /// Gets full detail of a rejected question with all review history.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="questionId">The question identifier.</param>
        /// <returns>Full detail of a rejected question with all review history.</returns>
        Task<CommonResponse<CorrectionDetailDto>> GetCorrectionDetailAsync(
            Guid userId, Guid questionId);

        /// <summary>
        /// Fixes a rejected question and resubmits for review.
        /// Re-increments TaskAssignment.CompletedCount.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="questionId">The question identifier.</param>
        /// <param name="dto">The dto.</param>
        /// <returns>True if the operation is successful; otherwise, false.</returns>
        Task<CommonResponse<bool>> FixAndResubmitAsync(
            Guid userId, Guid questionId, FixQuestionDto dto);
    }
}

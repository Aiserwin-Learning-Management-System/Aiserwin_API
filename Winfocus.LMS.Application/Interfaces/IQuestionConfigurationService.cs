namespace Winfocus.LMS.Application.Interfaces
{
    using System;
    using System.Threading.Tasks;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.QuestionConfig;

    /// <summary>
    /// Service interface for Question Configuration operations.
    /// </summary>
    public interface IQuestionConfigurationService
    {
        /// <summary>
        /// Generates a suggested Question Code without saving.
        /// </summary>
        /// <param name="dto">The hierarchy selection.</param>
        /// <returns>The suggested code wrapped in CommonResponse.</returns>
        Task<CommonResponse<SuggestedCodeResponseDto>> SuggestCodeAsync(SuggestQuestionCodeDto dto);

        /// <summary>
        /// Creates a new Question Configuration.
        /// </summary>
        /// <param name="dto">The creation data.</param>
        /// <param name="userId">The current user identifier.</param>
        /// <returns>The created configuration wrapped in CommonResponse.</returns>
        Task<CommonResponse<QuestionConfigurationDto>> CreateAsync(
            CreateQuestionConfigurationDto dto,
            Guid userId);

        /// <summary>
        /// Checks whether a Question Code is available.
        /// </summary>
        /// <param name="code">The code to check.</param>
        /// <returns>Availability status wrapped in CommonResponse.</returns>
        Task<CommonResponse<CodeAvailabilityDto>> CheckUniqueAsync(string code);

        /// <summary>
        /// Gets a Question Configuration by identifier.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        /// <returns>The configuration wrapped in CommonResponse.</returns>
        Task<CommonResponse<QuestionConfigurationDto>> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets filtered Question Configurations with pagination.
        /// </summary>
        /// <param name="request">The filter and pagination parameters.</param>
        /// <returns>Paged result wrapped in CommonResponse.</returns>
        Task<CommonResponse<PagedResult<QuestionConfigurationDto>>> GetFilteredAsync(
            QuestionConfigurationFilterRequest request);

        /// <summary>
        /// Soft deletes a Question Configuration.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="userId">The current user identifier.</param>
        /// <returns>Success status wrapped in CommonResponse.</returns>
        Task<CommonResponse<bool>> DeleteAsync(Guid id, Guid userId);
    }
}

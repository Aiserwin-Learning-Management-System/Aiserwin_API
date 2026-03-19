namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.QuestionTypeConfig;

    /// <summary>
    /// Service interface for Question Type Configuration operations.
    /// </summary>
    public interface IQuestionTypeConfigService
    {
        /// <summary>
        /// Creates a single Question Type Configuration.
        /// </summary>
        /// <param name="dto">The creation data.</param>
        /// <param name="userId">The current user identifier.</param>
        /// <returns>The created configuration wrapped in CommonResponse.</returns>
        Task<CommonResponse<QuestionTypeConfigDto>> CreateAsync(
            CreateQuestionTypeConfigDto dto,
            Guid userId);

        /// <summary>
        /// Creates multiple Question Type Configurations in a single operation.
        /// </summary>
        /// <param name="dto">The bulk creation data.</param>
        /// <param name="userId">The current user identifier.</param>
        /// <returns>List of created configurations wrapped in CommonResponse.</returns>
        Task<CommonResponse<List<QuestionTypeConfigDto>>> BulkCreateAsync(
            BulkCreateQuestionTypeConfigDto dto,
            Guid userId);

        /// <summary>
        /// Gets a Question Type Configuration by identifier.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        /// <returns>The configuration wrapped in CommonResponse.</returns>
        Task<CommonResponse<QuestionTypeConfigDto>> GetByIdAsync(Guid id);

        /// <summary>
        /// Updates an existing Question Type Configuration.
        /// </summary>
        /// <param name="id">The identifier to update.</param>
        /// <param name="dto">The updated data.</param>
        /// <param name="userId">The current user identifier.</param>
        /// <returns>The updated configuration wrapped in CommonResponse.</returns>
        Task<CommonResponse<QuestionTypeConfigDto>> UpdateAsync(
            Guid id,
            CreateQuestionTypeConfigDto dto,
            Guid userId);

        /// <summary>
        /// Soft deletes a Question Type Configuration.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="userId">The current user identifier.</param>
        /// <returns>Success status wrapped in CommonResponse.</returns>
        Task<CommonResponse<bool>> DeleteAsync(Guid id, Guid userId);

        /// <summary>
        /// Gets filtered Question Type Configurations with pagination.
        /// </summary>
        /// <param name="request">The filter and pagination parameters.</param>
        /// <returns>Paged result wrapped in CommonResponse.</returns>
        Task<CommonResponse<PagedResult<QuestionTypeConfigDto>>> GetFilteredAsync(
            QuestionTypeConfigFilterRequest request);

        /// <summary>
        /// Gets question types available for a specific hierarchy combination.
        /// Used for dropdown population in task assignment and question ID pages.
        /// </summary>
        /// <param name="query">The hierarchy combination.</param>
        /// <returns>List of question types wrapped in CommonResponse.</returns>
        Task<CommonResponse<List<QuestionTypeConfigDto>>> GetByHierarchyAsync(
            HierarchyQueryDto query);
    }
}

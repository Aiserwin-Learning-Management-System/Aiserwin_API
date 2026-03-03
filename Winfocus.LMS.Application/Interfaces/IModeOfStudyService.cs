namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.Masters;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Defines business operations for <see cref="ModeOfStudy"/> entities.
    /// </summary>
    public interface IModeOfStudyService
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>ModeOfStudyDto.</returns>
        Task<CommonResponse<List<ModeOfStudyDto>>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ModeOfStudyDto.</returns>
        Task<CommonResponse<ModeOfStudyDto>> GetByIdAsync(Guid id);

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>ModeOfStudyDto.</returns>
        Task<CommonResponse<ModeOfStudyDto>> CreateAsync(ModeOfStudyRequest request);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>id.</returns>
        Task<CommonResponse<ModeOfStudyDto>> UpdateAsync(Guid id, ModeOfStudyRequest request);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>id.</returns>
        Task<CommonResponse<bool>> DeleteAsync(Guid id);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="countryid">The identifier.</param>
        /// <returns>ModeOfStudyDto.</returns>
        Task<List<ModeOfStudyDto>> GetByCountryIdAsync(Guid countryid);

        /// <summary>
        /// Gets filtered mode of study with pagination support.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated mode of study result.</returns>
        Task<CommonResponse<PagedResult<ModeOfStudyDto>>> GetFilteredAsync(PagedRequest request);
    }
}

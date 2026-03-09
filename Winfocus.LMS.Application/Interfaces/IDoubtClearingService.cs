namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.Masters;

    /// <summary>
    /// Defines business operations for <see cref="DoubtClearing"/> entities.
    /// </summary>
    public interface IDoubtClearingService
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>DoubtClearingDto.</returns>
        Task<CommonResponse<List<DoubtClearingDto>>> GetAllAsync();
        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>DoubtClearingDto.</returns>
        Task<CommonResponse<DoubtClearingDto>> GetByIdAsync(Guid id);

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>DoubtClearingDto.</returns>
        Task<DoubtClearingDto> CreateAsync(DoubtClearingRequest request);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>id.</returns>
        Task<DoubtClearingDto> UpdateAsync(Guid id, DoubtClearingRequest request);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>id.</returns>
        Task<bool> DeleteAsync(Guid id);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="subjectid">The identifier.</param>
        /// <returns>DoubtClearingDto.</returns>
        Task<List<DoubtClearingDto>> GetBySubjectIdAsync(Guid subjectid);

        /// <summary>
        /// Gets filtered doubt clear result with pagination support.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated doubt clear result.</returns>
        Task<CommonResponse<PagedResult<DoubtClearingDto>>> GetFilteredAsync(PagedRequest request);

    }
}

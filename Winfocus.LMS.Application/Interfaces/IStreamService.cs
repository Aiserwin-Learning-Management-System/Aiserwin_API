namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.Masters;

    /// <summary>
    /// IStreamService.
    /// </summary>
    public interface IStreamService
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <param name="centerId">The centerId.</param>
        /// <returns>StreamDto list.</returns>
        Task<CommonResponse<List<StreamDto>>> GetAllAsync(Guid centerId);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>StreamDto.</returns>
        Task<CommonResponse<StreamDto>> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="centerId">The centerId.</param>
        /// <returns>StreamDto.</returns>
        Task<CommonResponse<StreamDto>> GetByIdCenterIdAsync(Guid id, Guid centerId);

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>StreamDto.</returns>
        Task<CommonResponse<StreamDto>> 
            CreateAsync(StreamRequest request);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>StreamDto.</returns>
        Task<CommonResponse<StreamDto>> UpdateAsync(Guid id, StreamRequest request);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="centerId">The centerId.</param>
        /// <returns>bool.</returns>
        Task<CommonResponse<bool>> DeleteAsync(Guid id, Guid centerId);

        /// <summary>
        /// Gets streams by grade identifier.
        /// </summary>
        /// <param name="gradeid">The grade identifier.</param>
        /// <returns>StreamDto list.</returns>
        Task<CommonResponse<List<StreamDto>>> GetByGradeIdAsync(Guid gradeid);

        /// <summary>
        /// Gets filtered streams with pagination support.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <param name="centerId">The centerId.</param>
        /// <returns>Paginated stream result.</returns>
        Task<CommonResponse<PagedResult<StreamDto>>> GetFilteredAsync(PagedRequest request, Guid centerId);
    }
}

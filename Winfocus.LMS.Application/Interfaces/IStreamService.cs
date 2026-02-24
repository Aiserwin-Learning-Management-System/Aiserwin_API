namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Masters;

    /// <summary>
    /// IStreamService.
    /// </summary>
    public interface IStreamService
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>StreamDto.</returns>
        Task<CommonResponse<List<StreamDto>>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>StreamDto.</returns>
        Task<CommonResponse<StreamDto>> GetByIdAsync(Guid id);

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>StreamDto.</returns>
        Task<StreamDto> CreateAsync(StreamRequest request);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>id.</returns>
        Task<StreamDto> UpdateAsync(Guid id, StreamRequest request);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>id.</returns>
        Task<bool> DeleteAsync(Guid id);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="gradeid">The identifier.</param>
        /// <returns>StreamDto.</returns>
        Task<List<StreamDto>> GetByGradeIdAsync(Guid gradeid);
    }
}

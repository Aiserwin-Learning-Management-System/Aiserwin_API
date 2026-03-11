using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.Masters;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// Defines business operations for <see cref="Batch"/> entities.
    /// </summary>
    public interface IBatchService
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <param name="centerId">The centerId.</param>
        /// <returns>BatchDto.</returns>
        Task<CommonResponse<List<BatchDto>>> GetAllAsync(Guid centerId);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>BatchDto.</returns>
        Task<CommonResponse<BatchDto>> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="centerId">The centerId.</param>
        /// <returns>BatchDto.</returns>
        Task<CommonResponse<BatchDto>> GetByIdCenterIdAsync(Guid id, Guid centerId);

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>BatchDto.</returns>
        Task<CommonResponse<BatchDto>> CreateAsync(BatchRequest request);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>id.</returns>
        Task<CommonResponse<BatchDto>> UpdateAsync(Guid id, BatchRequest request);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="centerId">The centerId.</param>
        /// <returns>id.</returns>
        Task<CommonResponse<bool>> DeleteAsync(Guid id, Guid centerId);

        /// <summary>
        /// Gets filtered batches with pagination support.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <param name="centerId">The centerId.</param>
        /// <returns>Paginated batches result.</returns>
        Task<CommonResponse<PagedResult<BatchDto>>> GetFilteredAsync(PagedRequest request, Guid centerId);
    }
}

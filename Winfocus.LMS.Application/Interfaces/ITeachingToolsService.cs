using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.DTOs.Teacher;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// Defines business operations for <see cref="TeachingTools"/> entities.
    /// </summary>
    public interface ITeachingToolsService
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>TeachingToolsResponseDto.</returns>
        Task<CommonResponse<List<TeachingToolsResponseDto>>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>TeachingToolsResponseDto.</returns>
        Task<CommonResponse<TeachingToolsResponseDto>> GetByIdAsync(Guid id);

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>TeachingToolsResponseDto.</returns>
        Task<CommonResponse<TeachingToolsResponseDto>> CreateAsync(TeachingToolsDto request);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>id.</returns>
        Task<CommonResponse<TeachingToolsResponseDto>> UpdateAsync(Guid id, TeachingToolsDto request);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>id.</returns>
        Task<CommonResponse<bool>> DeleteAsync(Guid id);

        /// <summary>
        /// Gets filtered TeachingToolsResponseDto with pagination support.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated TeachingToolsResponseDto result.</returns>
        Task<CommonResponse<PagedResult<TeachingToolsResponseDto>>> GetFilteredAsync(PagedRequest request);
    }
}

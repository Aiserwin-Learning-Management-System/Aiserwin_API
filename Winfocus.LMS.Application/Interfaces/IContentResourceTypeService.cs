using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.Masters;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// IContentResourceTypeService.
    /// </summary>
    public interface IContentResourceTypeService
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>ContentResourceTypeDto.</returns>
        Task<CommonResponse<List<ContentResourceTypeDto>>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ContentResourceTypeDto.</returns>
        Task<CommonResponse<ContentResourceTypeDto>> GetByIdAsync(Guid id);

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>ContentResourceTypeDto.</returns>
        Task<CommonResponse<ContentResourceTypeDto>> CreateAsync(ContentResourceTypeDto request);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>task.</returns>
        Task<CommonResponse<ContentResourceTypeDto>> UpdateAsync(Guid id, ContentResourceTypeDto request);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        Task<bool> DeleteAsync(Guid id);

        /// <summary>
        /// Gets ContentResourceType with pagination support.
        /// Search .
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>ContentResourceType result.</returns>
        Task<CommonResponse<PagedResult<ContentResourceTypeDto>>> GetFilteredAsync(
            PagedRequest request);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="chapterid">The chapterid.</param>
        /// <returns>ContentResourceTypeDto.</returns>
        Task<CommonResponse<List<ContentResourceTypeDto>>> GetByChapterIdAsync(Guid chapterid);
    }
}

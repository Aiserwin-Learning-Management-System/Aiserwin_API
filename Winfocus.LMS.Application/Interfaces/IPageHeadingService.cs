using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// Provides operations for managing page headings displayed in the admin UI.
    /// </summary>
    public interface IPageHeadingService
    {
        /// <summary>
        /// Retrieves all page headings available in the system.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="PageHeadingResponseDto"/> representing
        /// the main and sub headings configured for each page.
        /// </returns>
        Task<CommonResponse<List<PageHeadingResponseDto>>> GetAllAsync();

        /// <summary>
        /// Retrieves the heading details for a specific page using its unique page key.
        /// </summary>
        /// <param name="pageKey">
        /// The unique identifier of the page (for example: Dashboard, Students, Courses).
        /// </param>
        /// <returns>
        /// A <see cref="PageHeadingResponseDto"/> containing the main heading and sub heading of the page.
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown when the specified page key does not exist.
        /// </exception>
        Task<CommonResponse<PageHeadingResponseDto>> GetByKeyAsync(string pageKey);

        /// <summary>
        /// Updates the main heading and sub heading of a specific page.
        /// </summary>
        /// <param name="pageKey">
        /// The unique identifier of the page whose heading should be updated.
        /// </param>
        /// <param name="dto">
        /// The data transfer object containing the updated heading values.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous update operation.
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown when the provided page key does not exist.
        /// </exception>
        Task<CommonResponse<PageHeadingResponseDto>> UpdateAsync(string pageKey, UpdatePageHeadingDto dto);
    }
}

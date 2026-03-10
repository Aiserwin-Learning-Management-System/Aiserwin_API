using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.Masters;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// Defines business operations for <see cref="StaffCategory"/> entities.
    /// </summary>
    public interface IStaffCategoryService
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>StaffCategoryDto.</returns>
        Task<CommonResponse<List<StaffCategoryDto>>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>StaffCategoryDto.</returns>
        Task<CommonResponse<StaffCategoryDto>> GetByIdAsync(Guid id);

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>StaffCategoryDto.</returns>
        Task<CommonResponse<StaffCategoryDto>> CreateAsync(StaffCategoryRequestDto request);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>id.</returns>
        Task<CommonResponse<StaffCategoryDto>> UpdateAsync(Guid id, StaffCategoryRequestDto request);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>id.</returns>
        Task<CommonResponse<bool>> DeleteAsync(Guid id);

        /// <summary>
        /// Gets filtered academic year with pagination support.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated Staff category result.</returns>
        Task<CommonResponse<PagedResult<StaffCategoryDto>>> GetFilteredAsync(PagedRequest request);

    }
}

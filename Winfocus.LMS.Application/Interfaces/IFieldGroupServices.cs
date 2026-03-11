using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// IFieldGroupServices.
    /// </summary>
    public interface IFieldGroupServices
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>FieldGroupDto.</returns>
        Task<CommonResponse<List<FieldGroupDto>>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>FieldGroupDto.</returns>
        Task<CommonResponse<FieldGroupDto>> GetByIdAsync(Guid id);

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="userid">The userid.</param>
        /// <returns>FieldGroupDto.</returns>
        Task<CommonResponse<FieldGroupDto>> CreateAsync(CreateFieldGroupRequest request, Guid userid);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="userid">The userid.</param>
        /// <returns>id.</returns>
        Task<CommonResponse<FieldGroupDto>> UpdateAsync(Guid id, CreateFieldGroupRequest request, Guid userid);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>id.</returns>
        Task<CommonResponse<bool>> DeleteAsync(Guid id);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="name">The identifier.</param>
        /// <returns>FieldGroupDto.</returns>
        Task<bool> ExistsByNameAsync(string name);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="groupId">The identifier.</param>
        /// <returns>FieldGroupDto.</returns>
        Task<CommonResponse<FieldGroupFieldsResponseDto>> GetFieldsByGroupIdAsync(Guid groupId);

        /// <summary>
        /// Gets filtered FieldGroupDto with pagination support.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated FieldGroupDto result.</returns>
        Task<CommonResponse<PagedResult<FieldGroupDto>>> GetFilteredAsync(PagedRequest request);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.Masters;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// Defines business logic operations for managing DTP task assignments.
    /// </summary>
    public interface ITaskAssignmentService
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>TaskResponseDto.</returns>
        Task<CommonResponse<List<TaskResponseDto>>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>TaskResponseDto.</returns>
        Task<CommonResponse<TaskResponseDto>> GetByIdAsync(Guid id);

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>TaskResponseDto.</returns>
        Task<CommonResponse<TaskResponseDto>> CreateAsync(CreateTaskDto request);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>id.</returns>
        Task<CommonResponse<TaskResponseDto>> UpdateAsync(Guid id, CreateTaskDto request);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>id.</returns>
        Task<CommonResponse<bool>> DeleteAsync(Guid id);

        /// <summary>
        /// Gets filtered syllabuses with pagination support.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated syllabus result.</returns>
        Task<CommonResponse<PagedResult<TaskResponseDto>>> GetFilteredAsync(PagedRequest request);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="operatorid">The identifier.</param>
        /// <returns>TaskResponseDto.</returns>
        Task<CommonResponse<TaskResponseDto>> GetByOperatorIdAsync(Guid operatorid);

    }
}

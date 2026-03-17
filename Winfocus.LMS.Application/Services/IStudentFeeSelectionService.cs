using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;

namespace Winfocus.LMS.Application.Services
{
    /// <summary>
    /// IStudentFeeSelectionService.
    /// </summary>
    public interface IStudentFeeSelectionService
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>DoubtClearingDto.</returns>
        Task<CommonResponse<List<StudentFeeSelectionDto>>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>StudentFeeSelectionDto.</returns>
        Task<CommonResponse<StudentFeeSelectionDto>> GetByIdAsync(Guid id);

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>StudentFeeSelectionDto.</returns>
        Task<StudentFeeSelectionDto> CreateAsync(StudentFeeSelectionRequest request);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <exception cref="KeyNotFoundException"> created not found.</exception>
        /// <returns>task.</returns>
        Task<StudentFeeSelectionDto> UpdateAsync(Guid id, StudentFeeSelectionRequest request);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        Task<bool> DeleteAsync(Guid id);

        /// <summary>
        /// Gets filtered batch timing for monday to frida with pagination support.
        /// Search works on Subject name, Course Name, Stream Name, Grade Name, and Syllabus Name.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated doubt clear time result.</returns>
        Task<CommonResponse<PagedResult<StudentFeeSelectionDto>>> GetFilteredAsync(
            PagedRequest request);
    }
}

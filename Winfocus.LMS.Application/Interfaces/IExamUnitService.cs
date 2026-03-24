using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.Masters;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// IExamUnitService.
    /// </summary>
    public interface IExamUnitService
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>ExamUnitDto.</returns>
        Task<CommonResponse<List<ExamUnitDto>>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="subjectId">The identifier.</param>
        /// <returns>ExamUnitDto.</returns>
        Task<CommonResponse<ExamUnitDto>> GetByIdAsync(Guid id, Guid subjectId);

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>ExamUnitDto.</returns>
        Task<CommonResponse<ExamUnitDto>> CreateAsync(ExamUnitRequestDto request);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <exception cref="KeyNotFoundException">Exam units not found.</exception>
        /// <returns>task.</returns>
        Task<CommonResponse<ExamUnitDto>> UpdateAsync(Guid id, ExamUnitRequestDto request);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        Task<bool> DeleteAsync(Guid id);

        /// <summary>
        /// Gets filtered batch timing for monday to frida with pagination support.
        /// Search .
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated doubt clear time result.</returns>
        Task<CommonResponse<PagedResult<ExamUnitDto>>> GetFilteredAsync(PagedRequest request);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="subjectId">The subjectId.</param>
        /// <returns>ExamUnitDto.</returns>
        Task<CommonResponse<List<ExamUnitDto>>> GetBySubjectIdAsync(Guid subjectId);
    }
}

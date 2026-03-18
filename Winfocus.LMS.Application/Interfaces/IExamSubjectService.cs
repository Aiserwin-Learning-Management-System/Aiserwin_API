using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.Masters;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// IExamSubjectService.
    /// </summary>
    public interface IExamSubjectService
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>ExamSubjectDto.</returns>
        Task<CommonResponse<List<ExamSubjectDto>>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="gradeId">The identifier.</param>
        /// <returns>ExamSubjectDto.</returns>
        Task<CommonResponse<ExamSubjectDto>> GetByIdAsync(Guid id, Guid gradeId);

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>ExamSubjectDto.</returns>
        Task<CommonResponse<ExamSubjectDto>> CreateAsync(ExamSubjectRequestDto request);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <exception cref="KeyNotFoundException">Exam Subjects not found.</exception>
        /// <returns>task.</returns>
        Task<CommonResponse<ExamSubjectDto>> UpdateAsync(Guid id, ExamSubjectRequestDto request);

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
        Task<CommonResponse<PagedResult<ExamSubjectDto>>> GetFilteredAsync(
            PagedRequest request);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.Masters;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// IExamGradeService.
    /// </summary>
    public interface IExamGradeService
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>ExamGradeDto.</returns>
        Task<CommonResponse<List<ExamGradeDto>>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="syllabusID">The syllabusID.</param>
        /// <returns>ExamGradeDto.</returns>
        Task<CommonResponse<ExamGradeDto>> GetByIdAsync(Guid id, Guid syllabusID);

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>ExamGradeDto.</returns>
        Task<CommonResponse<ExamGradeDto>> CreateAsync(ExamGradeRequestDto request);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <exception cref="KeyNotFoundException">Exam Syllabus not found.</exception>
        /// <returns>task.</returns>
        Task<CommonResponse<ExamGradeDto>> UpdateAsync(Guid id, ExamGradeRequestDto request);

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
        Task<CommonResponse<PagedResult<ExamGradeDto>>> GetFilteredAsync(
            PagedRequest request);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="syllabusID">The syllabusID.</param>
        /// <returns>ExamGradeDto.</returns>
        Task<CommonResponse<List<ExamGradeDto>>> GetBySyllabusIdAsync(Guid syllabusID);
    }
}

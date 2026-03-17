using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.Masters;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// IExamSyllabusService.
    /// </summary>
    public interface IExamSyllabusService
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>ExamSyllabusDto.</returns>
        Task<CommonResponse<List<ExamSyllabusDto>>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="accademicYearId">The identifier.</param>
        /// <returns>ExamSyllabusDto.</returns>
        Task<CommonResponse<ExamSyllabusDto>> GetByIdAsync(Guid id, Guid accademicYearId);

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>ExamSyllabusDto.</returns>
        Task<CommonResponse<ExamSyllabusDto>> CreateAsync(ExamSyllabusRequestDto request);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <exception cref="KeyNotFoundException">Exam Syllabus not found.</exception>
        /// <returns>task.</returns>
        Task<CommonResponse<ExamSyllabusDto>> UpdateAsync(Guid id, ExamSyllabusRequestDto request);

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
        Task<CommonResponse<PagedResult<ExamSyllabusDto>>> GetFilteredAsync(
            PagedRequest request);

    }
}

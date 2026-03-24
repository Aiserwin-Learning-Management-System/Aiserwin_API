using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.Masters;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// IExamChapterService.
    /// </summary>
    public interface IExamChapterService
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>ExamChapterDto.</returns>
        Task<CommonResponse<List<ExamChapterDto>>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="unitid">The unitid.</param>
        /// <returns>ExamChapterDto.</returns>
        Task<CommonResponse<ExamChapterDto>> GetByIdAsync(Guid id, Guid unitid);

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>ExamChapterDto.</returns>
        Task<CommonResponse<ExamChapterDto>> CreateAsync(ExamChapterRequestDto request);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <exception cref="KeyNotFoundException">Exam units not found.</exception>
        /// <returns>task.</returns>
        Task<CommonResponse<ExamChapterDto>> UpdateAsync(Guid id, ExamChapterRequestDto request);

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
        /// <returns>Paginated ExamChapterDto.</returns>
        Task<CommonResponse<PagedResult<ExamChapterDto>>> GetFilteredAsync(PagedRequest request);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="unitId">The unitId.</param>
        /// <returns>ExamChapterDto.</returns>
        Task<CommonResponse<List<ExamChapterDto>>> GetByUnitIdAsync(Guid unitId);
    }
}

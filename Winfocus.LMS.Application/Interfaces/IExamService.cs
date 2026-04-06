using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.Exam;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// Defines business operations for <see cref="exam"/> entities.
    /// </summary>
    public interface IExamService
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>ExamDto.</returns>
        Task<CommonResponse<List<ExamDto>>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ExamDto.</returns>
        Task<CommonResponse<ExamDto>> GetByIdAsync(Guid id);

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>ExamDto.</returns>
        Task<CommonResponse<ExamDto>> CreateAsync(ExamRequest request);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>id.</returns>
        Task<CommonResponse<ExamDto>> UpdateAsync(Guid id, ExamRequest request);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>id.</returns>
        Task<CommonResponse<bool>> DeleteAsync(Guid id);

        /// <summary>
        /// Gets filtered Exam with pagination support.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated ExamDto result.</returns>
        Task<CommonResponse<PagedResult<ExamDto>>> GetFilteredAsync(PagedRequest request);

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <param name="examId">The paged examId.</param>
        /// <returns>ExamQuestion.</returns>
        Task<CommonResponse<List<ExamQuestionDto>>> GetQuestionsForExamAsync(Guid examId);

        /// <summary>
        /// Creates an exam-question mapping.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>ExamQuestion.</returns>
        Task<CommonResponse<ExamQuestionDto>> CreateExamQuestionAsync(ExamQuestionRequest request);

        /// <summary>
        /// Updates an existing exam-question mapping.
        /// </summary>
        /// <param name="id">The identifier of the mapping.</param>
        /// <param name="request">The request.</param>
        /// <returns>ExamQuestion.</returns>
        Task<CommonResponse<ExamQuestionDto>> UpdateExamQuestionAsync(Guid id, ExamQuestionRequest request);
    }
}

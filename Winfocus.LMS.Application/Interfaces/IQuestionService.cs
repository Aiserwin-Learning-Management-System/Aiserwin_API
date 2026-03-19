using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs.Question;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// Defines operations for managing operator questions.
    /// Handles creation, update, deletion, submission, retrieval, and preview.
    /// </summary>
    public interface IQuestionService
    {
        /// <summary>
        /// Creates a new question (MCQ or descriptive).
        /// </summary>
        /// <param name="dto">Question creation data.</param>
        /// <param name="operatorId">Current operator ID.</param>
        /// <returns>Created Question ID.</returns>
        Task<Guid> CreateAsync(CreateQuestionDto dto, Guid operatorId);

        /// <summary>
        /// Updates an existing question.
        /// Only allowed for Draft or Rejected questions.
        /// </summary>
        /// <param name="id">Question ID.</param>
        /// <param name="dto">Updated data.</param>
        /// <returns>.</returns>
        Task UpdateAsync(Guid id, CreateQuestionDto dto);

        /// <summary>
        /// Deletes a question.
        /// Only Draft questions can be deleted.
        /// </summary>
        /// <param name="id">Question ID.</param>
        /// <returns>.</returns>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Submits a draft question for review.
        /// </summary>
        /// <param name="id">Question ID.</param>
        /// <returns>.</returns>
        Task SubmitAsync(Guid id);

        /// <summary>
        /// Gets a question with options and review history.
        /// </summary>
        /// <param name="id">Question ID.</param>
        /// <returns>Detailed question response.</returns>
        Task<QuestionResponseDto> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets all questions for a task (paginated).
        /// </summary>
        /// <param name="taskId">Task ID.</param>
        /// <param name="page">Page number.</param>
        /// <param name="pageSize">Page size.</param>
        /// <returns>List of questions.</returns>
        Task<List<QuestionListDto>> GetByTaskIdAsync(Guid taskId, int page, int pageSize);

        /// <summary>
        /// Returns a formatted preview of a question.
        /// </summary>
        /// <param name="id">Question ID.</param>
        /// <returns>Preview DTO.</returns>
        Task<QuestionPreviewDto> PreviewAsync(Guid id);
    }
}

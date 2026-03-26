using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// Repository contract for managing <see cref="Question"/> entities.
    /// Provides data access methods for CRUD operations and
    /// task-based question retrieval.
    /// </summary>
    public interface IQuestionRepository
    {
        /// <summary>
        /// Gets a question by its identifier with options and reviews.
        /// </summary>
        /// <param name="id">The question identifier.</param>
        /// <returns>The question if found; otherwise null.</returns>
        Task<Question?> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets all questions for a specific task with pagination.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <param name="page">The page number (1-based).</param>
        /// <param name="pageSize">The number of records per page.</param>
        /// <returns>List of questions.</returns>
        Task<List<Question>> GetByTaskIdAsync(Guid taskId, int page, int pageSize);

        /// <summary>
        /// Gets total question count for a task.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <returns>Total number of questions.</returns>
        Task<int> GetCountByTaskIdAsync(Guid taskId);

        /// <summary>
        /// Adds a new question.
        /// </summary>
        /// <param name="question">The question entity.</param>
        /// <returns>.</returns>
        Task AddAsync(Question question);

        /// <summary>
        /// Updates an existing question.
        /// </summary>
        /// <param name="question">The question entity.</param>
        /// <returns>.</returns>
        Task UpdateAsync(Question question);

        /// <summary>
        /// Deletes a question.
        /// </summary>
        /// <param name="question">The question entity.</param>
        /// <returns>.</returns>
        Task DeleteAsync(Question question);

        /// <summary>
        /// Checks whether a question exists.
        /// </summary>
        /// <param name="id">The question identifier.</param>
        /// <returns>True if exists; otherwise false.</returns>
        Task<bool> ExistsAsync(Guid id);

        /// <summary>
        /// Retrieves questions for a specific operator with optional filters, sorting and pagination.
        /// Returns items and the total count matching the filters.
        /// </summary>
        Task<(List<Question> Items, int TotalCount)> GetByOperatorAsync(
            Guid operatorId,
            string? subject,
            string? chapter,
            int? status,
            int? questionType,
            string? search,
            string? sortBy,
            int pageNumber,
            int pageSize);

        /// <summary>
        /// Gets question statistics breakdown for an operator.
        /// </summary>
        Task<Winfocus.LMS.Application.DTOs.Stats.QuestionStatsDto> GetStatsForOperatorAsync(Guid operatorId);

        /// <summary>
        /// Gets queryable for filtering with full hierarchy.
        /// </summary>
        /// <returns>Queryable ExamUnit.</returns>
        IQueryable<Question> Query();
    }
}

namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Repository interface for <see cref="QuestionConfiguration"/> data access.
    /// </summary>
    public interface IQuestionConfigurationRepository
    {
        /// <summary>
        /// Gets the next sequence number for the given scope.
        /// </summary>
        /// <param name="syllabusId">The syllabus identifier.</param>
        /// <param name="academicYearId">The academic year identifier.</param>
        /// <param name="gradeId">The grade identifier.</param>
        /// <param name="subjectId">The subject identifier.</param>
        /// <param name="unitId">The unit identifier.</param>
        /// <param name="chapterId">The chapter identifier.</param>
        /// <param name="questionTypeId">The question type identifier.</param>
        /// <returns>The next available sequence number.</returns>
        Task<int> GetNextSequenceAsync(
            Guid syllabusId,
            Guid academicYearId,
            Guid gradeId,
            Guid subjectId,
            Guid unitId,
            Guid chapterId,
            Guid questionTypeId);

        /// <summary>
        /// Checks whether a Question Code already exists.
        /// </summary>
        /// <param name="questionCode">The Question Code to check.</param>
        /// <returns>True if the code exists; otherwise false.</returns>
        Task<bool> CodeExistsAsync(string questionCode);

        /// <summary>
        /// Adds a new Question Configuration.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The created entity.</returns>
        Task<QuestionConfiguration> AddAsync(QuestionConfiguration entity);

        /// <summary>
        /// Gets a Question Configuration by identifier with all navigations.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        /// <returns>The entity if found; otherwise null.</returns>
        Task<QuestionConfiguration?> GetByIdAsync(Guid id);

        /// <summary>
        /// Returns a queryable for building filtered queries.
        /// Includes all 8 navigation properties.
        /// </summary>
        /// <returns>An IQueryable of QuestionConfiguration.</returns>
        IQueryable<QuestionConfiguration> Query();

        /// <summary>
        /// Performs a soft delete.
        /// </summary>
        /// <param name="id">The identifier of the record to delete.</param>
        /// <param name="userId">The user performing the delete.</param>
        /// <returns>True if deleted; false if not found.</returns>
        Task<bool> DeleteAsync(Guid id, Guid userId);
    }
}

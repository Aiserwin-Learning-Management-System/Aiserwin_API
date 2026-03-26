namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Repository interface for <see cref="QuestionTypeConfig"/> data access.
    /// </summary>
    public interface IQuestionTypeConfigRepository
    {
        /// <summary>
        /// Adds a new Question Type Configuration.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The created entity.</returns>
        Task<QuestionTypeConfig> AddAsync(QuestionTypeConfig entity);

        /// <summary>
        /// Adds multiple Question Type Configurations in a single operation.
        /// </summary>
        /// <param name="entities">The entities to add.</param>
        /// <returns>The created entities.</returns>
        Task<List<QuestionTypeConfig>> AddRangeAsync(List<QuestionTypeConfig> entities);

        /// <summary>
        /// Gets a Question Type Configuration by identifier with all navigations loaded.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        /// <returns>The entity if found; otherwise null.</returns>
        Task<QuestionTypeConfig?> GetByIdAsync(Guid id);

        /// <summary>
        /// Updates an existing Question Type Configuration.
        /// </summary>
        /// <param name="entity">The entity with updated values.</param>
        /// <returns>The updated entity.</returns>
        Task<QuestionTypeConfig> UpdateAsync(QuestionTypeConfig entity);

        /// <summary>
        /// Performs a soft delete on a Question Type Configuration.
        /// </summary>
        /// <param name="id">The identifier of the record to delete.</param>
        /// <param name="userId">The user performing the delete.</param>
        /// <returns>True if deleted; false if not found.</returns>
        Task<bool> DeleteAsync(Guid id, Guid userId);

        /// <summary>
        /// Checks whether a question type name already exists for the given hierarchy combination.
        /// </summary>
        /// <param name="syllabusId">The syllabus identifier.</param>
        /// <param name="gradeId">The grade identifier.</param>
        /// <param name="subjectId">The subject identifier.</param>
        /// <param name="unitId">The unit identifier.</param>
        /// <param name="chapterId">The chapter identifier.</param>
        /// <param name="resourceTypeId">The resource type identifier.</param>
        /// <param name="name">The question type name to check.</param>
        /// <param name="excludeId">Optional ID to exclude (for updates).</param>
        /// <returns>True if a duplicate exists; otherwise false.</returns>
        Task<bool> IsDuplicateAsync(
            Guid syllabusId,
            Guid gradeId,
            Guid subjectId,
            Guid unitId,
            Guid chapterId,
            Guid resourceTypeId,
            string name,
            Guid? excludeId = null);

        /// <summary>
        /// Gets question types available for a specific hierarchy combination.
        /// Used for dropdown population in task assignment and question ID configuration.
        /// </summary>
        /// <param name="syllabusId">The syllabus identifier.</param>
        /// <param name="gradeId">The grade identifier.</param>
        /// <param name="subjectId">The subject identifier.</param>
        /// <param name="unitId">The unit identifier.</param>
        /// <param name="chapterId">The chapter identifier.</param>
        /// <param name="resourceTypeId">The resource type identifier.</param>
        /// <returns>List of active question types for the hierarchy.</returns>
        Task<List<QuestionTypeConfig>> GetByHierarchyAsync(
            Guid syllabusId,
            Guid gradeId,
            Guid subjectId,
            Guid unitId,
            Guid chapterId,
            Guid resourceTypeId);

        /// <summary>
        /// Returns a queryable for building filtered queries. Includes all 6 navigations.
        /// </summary>
        /// <returns>An IQueryable of QuestionTypeConfig.</returns>
        IQueryable<QuestionTypeConfig> Query();
    }
}

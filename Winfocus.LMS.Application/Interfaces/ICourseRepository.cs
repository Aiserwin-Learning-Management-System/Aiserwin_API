namespace Winfocus.LMS.Application.Interfaces
{
    using System;
    using System.Collections.Generic;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// ICourseRepository.
    /// </summary>
    public interface ICourseRepository
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>Course.</returns>
        Task<IReadOnlyList<Course>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Course.</returns>
        Task<Course?> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets the by stream asynchronous.
        /// </summary>
        /// <param name="streamId">The stream identifier.</param>
        /// <returns>Course.</returns>
        Task<IReadOnlyList<Course>> GetByStreamAsync(Guid streamId);

        /// <summary>
        /// Gets the by subject asynchronous.
        /// </summary>
        /// <param name="subjectId">The subject identifier.</param>
        /// <returns>Course.</returns>
        Task<IReadOnlyList<Course>> GetBySubjectAsync(Guid subjectId);

        /// <summary>
        /// Gets the by identifier with subjects asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Course.</returns>
        Task<Course?> GetByIdWithSubjectsAsync(Guid id);

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="course">The course.</param>
        /// <returns>Course.</returns>
        Task<Course> AddAsync(Course course);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="course">The course.</param>
        /// <returns>Course.</returns>
        Task UpdateAsync(Course course);

        /// <summary>
        /// Softs the delete asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Course.</returns>
        Task SoftDeleteAsync(Guid id);
    }
}

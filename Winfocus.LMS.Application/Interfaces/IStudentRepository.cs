using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// Defines CRUD operations for <see cref="Student"/> entities.
    /// </summary>
    public interface IStudentRepository
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>Student.</returns>
        Task<IReadOnlyList<Student>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Student.</returns>
        Task<Student?> GetByIdAsync(Guid id);

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="Student">The Student.</param>
        /// <returns>Student.</returns>
        Task<Student> AddAsync(Student Student);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="student">The state.</param>
        /// <returns>Student.</returns>
        Task UpdateAsync(Student student);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        Task DeleteAsync(Guid id);

    }
}

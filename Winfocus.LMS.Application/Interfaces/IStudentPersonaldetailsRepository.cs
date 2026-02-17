using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// Defines CRUD operations for <see cref="StudentPersonaldetails"/> entities.
    /// </summary>
    public interface IStudentPersonaldetailsRepository
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>StudentPersonalDetails.</returns>
        Task<IReadOnlyList<StudentPersonalDetails>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>StudentPersonalDetails.</returns>
        Task<StudentPersonalDetails?> GetByIdAsync(Guid id);

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="studentPersonalDetails">The StudentPersonalDetails.</param>
        /// <returns>StudentPersonalDetails.</returns>
        Task<StudentPersonalDetails> AddAsync(StudentPersonalDetails studentPersonalDetails);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="studentPersonalDetails">The state.</param>
        /// <returns>StudentPersonalDetails.</returns>
        Task UpdateAsync(StudentPersonalDetails studentPersonalDetails);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        Task DeleteAsync(Guid id);
    }
}

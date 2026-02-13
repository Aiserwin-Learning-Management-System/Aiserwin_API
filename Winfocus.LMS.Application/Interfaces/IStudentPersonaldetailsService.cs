using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs.Students;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// Defines business operations for <see cref="StudentPersonaldetails"/> entities.
    /// </summary>
    public interface IStudentPersonaldetailsService
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>StudentPersonaldetailsdto.</returns>
        Task<IReadOnlyList<StudentPersonaldetailsdto>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>StudentPersonaldetailsdto.</returns>
        Task<StudentPersonaldetailsdto?> GetByIdAsync(Guid id);

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>StudentPersonaldetailsdto.</returns>
        Task<StudentPersonaldetailsdto> CreateAsync(StudentPersonaldetailsRequest request);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>id.</returns>
        Task UpdateAsync(Guid id, StudentPersonaldetailsRequest request);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>id.</returns>
        Task DeleteAsync(Guid id);
    }
}

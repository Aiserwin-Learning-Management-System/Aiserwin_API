using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// Defines business operations for <see cref="StaffCategory"/> entities.
    /// </summary>
    public interface IStaffCategoryRepository
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>StaffCategory.</returns>
        Task<IReadOnlyList<StaffCategory>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Staff categories.</returns>
        Task<StaffCategory?> GetByIdAsync(Guid id);

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="staffCategory">The StaffCategory.</param>
        /// <returns>StaffCategory.</returns>
        Task<StaffCategory> AddAsync(StaffCategory staffCategory);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="staffCategory">The staffCategory.</param>
        /// <returns>staffCategory.</returns>
        Task<StaffCategory> UpdateAsync(StaffCategory staffCategory);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        Task<bool> DeleteAsync(Guid id);

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>staffcategory.</returns>
        IQueryable<StaffCategory> Query();
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// Defines business operations for <see cref="StudentFeeSelection"/> entities.
    /// </summary>
    public interface IStudentFeeSelectionRepository
    {
        /// <summary>
        /// Gets the fee plan by identifier asynchronous.
        /// </summary>
        /// <param name="feeselectionId">The student fee selection identifier.</param>
        /// <returns>Task&lt;FeePlan?&gt;.</returns>
        Task<StudentFeeSelection?> GetFeePlanByIdAsync(Guid feeselectionId);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>StudentFeeSelection.</returns>
        Task<StudentFeeSelection?> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <returns>StudentFeeSelection.</returns>
        Task<List<StudentFeeSelection>> GetAllAsync();

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="studentfeeselection">The StudentFeeSelection.</param>
        /// <returns>StudentFeeSelection.</returns>
        Task<StudentFeeSelection> AddAsync(StudentFeeSelection studentfeeselection);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="studentfeeselection">The studentfeeselection.</param>
        /// <returns>StudentFeeSelection.</returns>
        Task<StudentFeeSelection> UpdateAsync(StudentFeeSelection studentfeeselection);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>StudentFeeSelection.</returns>
        Task<bool> DeleteAsync(Guid id);

        /// <summary>
        /// Gets queryable for filtering with full hierarchy.
        /// </summary>
        /// <returns>Queryable StudentFeeSelection.</returns>
        IQueryable<StudentFeeSelection> Query();
    }
}

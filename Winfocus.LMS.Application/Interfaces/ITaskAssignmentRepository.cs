using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// Defines database operations for managing DTP task assignments.
    /// </summary>
    public interface ITaskAssignmentRepository
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>TaskAssignment.</returns>
        Task<IReadOnlyList<TaskAssignment>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>TaskAssignment.</returns>
        Task<TaskAssignment?> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="operatorid">The identifier.</param>
        /// <returns>TaskAssignment.</returns>
        Task<List<TaskAssignment?>> GetByOperatorIdAsync(Guid operatorid);

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="taskassignment">The TaskAssignment.</param>
        /// <returns>TaskAssignment.</returns>
        Task<TaskAssignment> AddAsync(TaskAssignment taskassignment);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="taskassignment">The taskassignment.</param>
        /// <returns>taskassignment.</returns>
        Task<TaskAssignment> UpdateAsync(TaskAssignment taskassignment);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        Task<bool> DeleteAsync(Guid id);

        /// <summary>
        /// Existses the by name asynchronous.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>bool.</returns>
        Task<bool> ExistsByNameAsync(string name);

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>TaskAssignment.</returns>
        IQueryable<TaskAssignment> Query();

        /// <summary>
        /// Retrieves all task assignments including related operator data
        /// for dashboard overview calculations.
        /// </summary>
        /// <returns>
        /// A list of <see cref="TaskAssignment"/> entities.
        /// </returns>
        Task<List<TaskAssignment>> GetAllForOverviewAsync();
    }
}

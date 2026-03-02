namespace Winfocus.LMS.Application.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Defines CRUD operations for <see cref="ModeOfStudy"/> entities.
    /// </summary>
    public interface IModeOfStudyRepository
    {/// <summary>
     /// Gets all asynchronous.
     /// </summary>
     /// <returns>ModeOfStudy.</returns>
        Task<IReadOnlyList<ModeOfStudy>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ModeOfStudy.</returns>
        Task<ModeOfStudy?> GetByIdAsync(Guid id);

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="modeofstudy">The mode of study.</param>
        /// <returns>modeofstudy.</returns>
        Task<ModeOfStudy> AddAsync(ModeOfStudy modeofstudy);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="modeofstudy">The modeofstudy.</param>
        /// <returns>modeofstudy.</returns>
        Task<ModeOfStudy> UpdateAsync(ModeOfStudy modeofstudy);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        Task<bool> DeleteAsync(Guid id);

        /// <summary>
        /// Existses the by code asynchronous.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns>bool.</returns>
        Task<bool> ExistsByCodeAsync(string code);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="stateid">The identifier.</param>
        /// <returns>State.</returns>
        Task<List<ModeOfStudy>> GetByStateIdAsync(Guid stateid);

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>modeofstudy.</returns>
        IQueryable<ModeOfStudy> Query();
    }
}

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
        /// <param name="countryId">The countryId.</param>
        /// <returns>ModeOfStudy.</returns>
        Task<ModeOfStudy?> GetByIdAsync(Guid id, Guid countryId);

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
        /// <param name="countryId">The countryId.</param>
        /// <returns>task.</returns>
        Task<bool> DeleteAsync(Guid id, Guid countryId);

        /// <summary>
        /// Existses the by code asynchronous.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns>bool.</returns>
        Task<bool> ExistsByCodeAsync(string code);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="countryid">The identifier.</param>
        /// <returns>State.</returns>
        Task<List<ModeOfStudy>> GetByCountryIdAsync(Guid countryid);

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <param name="countryId">The identifier.</param>
        /// <returns>modeofstudy.</returns>
        IQueryable<ModeOfStudy> Query(Guid countryId);
    }
}

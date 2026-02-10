namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Masters;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Defines business operations for <see cref="ModeOfStudy"/> entities.
    /// </summary>
    public interface IModeOfStudyService
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>ModeOfStudyDto.</returns>
        Task<IReadOnlyList<ModeOfStudyDto>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ModeOfStudyDto.</returns>
        Task<ModeOfStudyDto?> GetByIdAsync(Guid id);

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>ModeOfStudyDto.</returns>
        Task<ModeOfStudyDto> CreateAsync(ModeOfStudyRequest request);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>id.</returns>
        Task UpdateAsync(Guid id, ModeOfStudyRequest request);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>id.</returns>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="stateid">The identifier.</param>
        /// <returns>ModeOfStudyDto.</returns>
        Task<IReadOnlyList<ModeOfStudyDto>> GetByStateIdAsync(Guid stateid);
    }
}

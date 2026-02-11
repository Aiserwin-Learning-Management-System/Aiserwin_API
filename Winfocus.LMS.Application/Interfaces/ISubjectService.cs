namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Masters;

    /// <summary>
    /// Subject service contract.
    /// </summary>
    public interface ISubjectService
    {
        /// <summary>
        /// Gets all active subjects.
        /// </summary>
        /// <returns>A list of subject DTOs.</returns>
        Task<IReadOnlyList<SubjectDto>> GetAllAsync();

        /// <summary>
        /// Gets a subject by identifier.
        /// </summary>
        /// <param name="id">The subject identifier.</param>
        /// <returns>The subject DTO if found; otherwise, null.</returns>
        Task<SubjectDto?> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets subjects by stream identifier.
        /// </summary>
        /// <param name="streamId">The stream identifier.</param>
        /// <returns>A list of subject DTOs associated with the specified stream.</returns>
        Task<IReadOnlyList<SubjectDto>> GetByStreamAsync(Guid streamId);

        /// <summary>
        /// Creates a new subject.
        /// </summary>
        /// <param name="request">The subject creation request.</param>
        /// <returns>The created subject DTO.</returns>
        Task<SubjectDto> CreateAsync(SubjectRequest request);

        /// <summary>
        /// Updates an existing subject.
        /// </summary>
        /// <param name="id">The subject identifier.</param>
        /// <param name="request">The subject update request.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateAsync(Guid id, SubjectRequest request);

        /// <summary>
        /// Soft deletes a subject.
        /// </summary>
        /// <param name="id">The subject identifier.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteAsync(Guid id);
    }
}

namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// ISyllabusRepository.
    /// </summary>
    public interface ISyllabusRepository
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>Syllabus.</returns>
        Task<IReadOnlyList<Syllabus>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Syllabus.</returns>
        Task<Syllabus?> GetByIdAsync(Guid id);

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="syllabus">The syllabus.</param>
        /// <returns>Syllabus.</returns>
        Task<Syllabus> AddAsync(Syllabus syllabus);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="syllabus">The syllabus.</param>
        /// <returns>Syllabus.</returns>
        Task<Syllabus> UpdateAsync(Syllabus syllabus);

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
        /// <param name="centerid">The identifier.</param>
        /// <returns>Syllabus.</returns>
        Task<List<Syllabus>> GetByCenterIdAsync(Guid centerid);

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>syllabuses.</returns>
        IQueryable<Syllabus> Query();
    }
}
